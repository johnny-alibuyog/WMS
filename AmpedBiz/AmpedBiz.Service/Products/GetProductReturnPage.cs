using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductReturnPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductOrderReturnPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<ReturnItem>();

                    // compose filters
                    message.Filter.Compose<Guid>("id", value =>
                    {
                        query = query.Where(x => x.Product.Id == value);
                    });

                    // compose sort order
                    message.Sorter.Compose("reason", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReturnReason.Name)
                            : query.OrderByDescending(x => x.ReturnReason.Name);
                    });

                    message.Sorter.Compose("returnedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Return.ReturnedOn)
                            : query.OrderByDescending(x => x.Return.ReturnedOn);
                    });

                    message.Sorter.Compose("returnedBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.Return.ReturnedBy.Person.FirstName)
                                .OrderBy(x => x.Return.ReturnedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.Return.ReturnedBy.Person.FirstName)
                                .OrderByDescending(x => x.Return.ReturnedBy.Person.LastName);
                    });

                    message.Sorter.Compose("returned", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.TotalPrice.Amount)
                            : query.OrderByDescending(x => x.TotalPrice.Amount);
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
                            Id = x.Return.Id,
                            ReasonName = x.ReturnReason.Name,
                            ReturnedOn = x.Return.ReturnedOn,
                            ReturnedByName =
                                x.Return.ReturnedBy.Person.FirstName + " " +
                                x.Return.ReturnedBy.Person.LastName,
                            ReturnedAmount = x.TotalPrice.Amount,
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

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
