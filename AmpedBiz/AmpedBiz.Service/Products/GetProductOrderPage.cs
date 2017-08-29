using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductOrderPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductOrderPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<OrderItem>();

                    // compose filters
                    message.Filter.Compose<Guid>("id", value =>
                    {
                        query = query.Where(x => x.Product.Id == value);
                    });

                    // compose sort order
                    message.Sorter.Compose("orderNumber", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Order.OrderNumber)
                            : query.OrderByDescending(x => x.Order.OrderNumber);
                    });

                    message.Sorter.Compose("createdOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Order.CreatedOn)
                            : query.OrderByDescending(x => x.Order.CreatedOn);
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Order.Status)
                            : query.OrderByDescending(x => x.Order.Status);
                    });

                    message.Sorter.Compose("customer", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Order.Customer.Name)
                            : query.OrderByDescending(x => x.Order.Customer.Name);
                    });

                    message.Sorter.Compose("quantityUnit", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Quantity.Unit.Name)
                            : query.OrderByDescending(x => x.Quantity.Unit.Name);
                    });

                    message.Sorter.Compose("quantityValue", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Quantity.Value)
                            : query.OrderByDescending(x => x.Quantity.Value);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.ProductOrderPageItem()
                        {
                            Id = x.Order.Id,
                            OrderNumber = x.Order.OrderNumber,
                            CreatedOn = x.Order.CreatedOn,
                            Status = x.Order.Status.ToString(),
                            CustomerName = x.Order.Customer.Name,
                            Quantity = new Dto.Measure(
                                x.Quantity.Value,
                                new Dto.UnitOfMeasure(
                                    x.Quantity.Unit.Id,
                                    x.Quantity.Unit.Name
                                )
                            )
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