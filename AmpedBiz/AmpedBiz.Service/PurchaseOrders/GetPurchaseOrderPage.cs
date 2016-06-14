using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.PurchaseOrderPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<PurchaseOrder>();

                    // compose filters
                    message.Filter.Compose<PurchaseOrderStatus[]>("statuses", value =>
                    {
                        query = query.Where(x => value.Contains(x.Status));
                    });

                    // compose filters
                    message.Filter.Compose<PurchaseOrderStatus>("status", value =>
                    {
                        query = query.Where(x => x.Status == value);
                    });

                    message.Filter.Compose<string>("supplier", value =>
                    {
                        query = query.Where(x => x.Supplier.Id == value);
                    });

                    // compose sort
                    message.Sorter.Compose("supplier", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Supplier.Name)
                            : query.OrderByDescending(x => x.Supplier.Name);
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Status)
                            : query.OrderByDescending(x => x.Status);
                    });

                    message.Sorter.Compose("createdBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.CreatedBy.User.Person.FirstName)
                                .ThenBy(x => x.CreatedBy.User.Person.LastName)
                            : query
                                .OrderByDescending(x => x.CreatedBy.User.Person.LastName)
                                .ThenByDescending(x => x.CreatedBy.User.Person.LastName);
                    });

                    message.Sorter.Compose("createdOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.CreatedOn)
                            : query.OrderByDescending(x => x.CreatedOn);
                    });

                    message.Sorter.Compose("submittedBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.SubmittedBy.User.Person.FirstName)
                                .ThenBy(x => x.SubmittedBy.User.Person.LastName)
                            : query
                                .OrderByDescending(x => x.SubmittedBy.User.Person.LastName)
                                .ThenByDescending(x => x.SubmittedBy.User.Person.LastName);
                    });

                    message.Sorter.Compose("submittedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.SubmittedOn)
                            : query.OrderByDescending(x => x.SubmittedOn);
                    });

                    message.Sorter.Compose("payedBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.PayedBy.User.Person.FirstName)
                                .ThenBy(x => x.PayedBy.User.Person.LastName)
                            : query
                                .OrderByDescending(x => x.PayedBy.User.Person.LastName)
                                .ThenByDescending(x => x.PayedBy.User.Person.LastName);
                    });

                    message.Sorter.Compose("payedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PayedOn)
                            : query.OrderByDescending(x => x.PayedOn);
                    });

                    message.Sorter.Compose("total", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Total.Amount)
                            : query.OrderByDescending(x => x.Total.Amount);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.PurchaseOrderPageItem()
                        {
                            Id = x.Id,
                            Supplier = x.Supplier.Name,
                            Status = x.Status.ToString(),
                            CreatedBy = 
                                x.CreatedBy.User.Person.FirstName + " " +
                                x.CreatedBy.User.Person.LastName,
                            CreatedOn = x.CreatedOn,
                            SubmittedBy =
                                x.SubmittedBy.User.Person.FirstName + " " + 
                                x.SubmittedBy.User.Person.LastName,
                            SubmittedOn = x.SubmittedOn,
                            PayedBy = 
                                x.PayedBy.User.Person.FirstName + " " +
                                x.PayedBy.User.Person.LastName,
                            PayedOn = x.PayedOn,
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