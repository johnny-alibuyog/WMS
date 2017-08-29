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
    public class GetProductOrderReturnPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductOrderReturnPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<OrderReturn>();

                    // compose filters
                    message.Filter.Compose<Guid>("id", value =>
                    {
                        query = query.Where(x => x.Product.Id == value);
                    });

                    // compose sort order
                    message.Sorter.Compose("reason", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Reason.Name)
                            : query.OrderByDescending(x => x.Reason.Name);
                    });

                    message.Sorter.Compose("returnedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReturnedOn)
                            : query.OrderByDescending(x => x.ReturnedOn);
                    });

                    message.Sorter.Compose("returnedBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.ReturnedBy.Person.FirstName)
                                .OrderBy(x => x.ReturnedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.ReturnedBy.Person.FirstName)
                                .OrderByDescending(x => x.ReturnedBy.Person.LastName);
                    });

                    message.Sorter.Compose("returned", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Returned.Amount)
                            : query.OrderByDescending(x => x.Returned.Amount);
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
                        .Select(x => new Dto.ProductOrderReturnPageItem()
                        {
                            Id = x.Order.Id,
                            ReasonName = x.Reason.Name,
                            ReturnedOn = x.ReturnedOn,
                            ReturnedByName =
                                x.ReturnedBy.Person.FirstName + " " +
                                x.ReturnedBy.Person.LastName,
                            ReturnedAmount = x.Returned.Amount,
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
