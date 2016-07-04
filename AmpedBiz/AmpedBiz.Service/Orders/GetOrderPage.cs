using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.OrderPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Order>();

                    // compose filters

                    message.Filter.Compose<string>("status", value =>
                    {
                        query = query.Where(x => x.Status.ToString().ToLower().Contains(value.ToLower()));
                    });


                    message.Filter.Compose<string>("customer", value =>
                    {
                        query = query.Where(x => x.Customer.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Status)
                            : query.OrderByDescending(x => x.Status);
                    });

                    message.Sorter.Compose("createdOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.OrderDate)
                            : query.OrderByDescending(x => x.OrderDate);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.OrderPageItem()
                        {
                            Id = x.Id,
                            CreatedByName = x.CreatedBy.FullName(),
                            CustomerName = x.Customer.Name,
                            OrderDate = x.OrderDate.Value,
                            ShippingFee = x.ShippingFee.ToStringWithSymbol(),
                            StatusName = x.Status.ToString(),
                            SubTotal = x.SubTotal.ToStringWithSymbol(),
                            Tax = x.Tax.ToStringWithSymbol(),
                            Total = x.Total.ToStringWithSymbol()
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };
                }

                return response;
            }
        }
    }
}