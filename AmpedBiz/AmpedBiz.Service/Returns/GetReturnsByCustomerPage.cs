using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturnsByCustomerPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ReturnsByCustomerPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Return>()
                        .GroupBy(x => x.Customer.Id)
                        .Select(x => new Dto.ReturnsByCustomerPageItem()
                        {
                            Id = x.Key,
                            CustomerName = x.Max(o => o.Customer.Name),
                            TotalAmount = x.Sum(o => o.Total.Amount)
                        });

                    // compose filters
                    message.Filter.Compose<Guid>("customer", value =>
                    {
                        query = query.Where(x => x.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("customer", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.CustomerName)
                            : query.OrderByDescending(x => x.CustomerName);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.TotalAmount)
                            : query.OrderByDescending(x => x.TotalAmount);
                    });

                    var itemsFuture = query
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
