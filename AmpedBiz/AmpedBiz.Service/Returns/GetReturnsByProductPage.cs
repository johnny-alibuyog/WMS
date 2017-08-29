using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturnsByProductPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ReturnsByProductPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<ReturnItem>()
                        .GroupBy(x => x.Product.Id)
                        .Select(x => new Dto.ReturnsByProductPageItem()
                        {
                            Id = x.Key,
                            ProductCode = x.Max(o => o.Product.Code),
                            ProductName = x.Max(o => o.Product.Name),
                            QuantityValue = x.Sum(o => o.Quantity.Value),
                            TotalAmount = x.Sum(o => o.TotalPrice.Amount)
                        });

                    // compose filters
                    message.Filter.Compose<Guid>("product", value =>
                    {
                        query = query.Where(x => x.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("product", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ProductName)
                            : query.OrderByDescending(x => x.ProductName);
                    });

                    message.Sorter.Compose("quantityValue", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.QuantityValue)
                            : query.OrderByDescending(x => x.QuantityValue);
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
