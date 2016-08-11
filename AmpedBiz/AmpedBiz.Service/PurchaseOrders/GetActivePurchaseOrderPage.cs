using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetActivePurchaseOrderPage
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
                    var query = session.Query<PurchaseOrder>().Where(x =>
                        x.Status != PurchaseOrderStatus.Cancelled &&
                        x.Status != PurchaseOrderStatus.Completed
                    );

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
                                .OrderBy(x => x.CreatedBy.Person.FirstName)
                                .ThenBy(x => x.CreatedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.CreatedBy.Person.LastName)
                                .ThenByDescending(x => x.CreatedBy.Person.LastName);
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
                                .OrderBy(x => x.SubmittedBy.Person.FirstName)
                                .ThenBy(x => x.SubmittedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.SubmittedBy.Person.LastName)
                                .ThenByDescending(x => x.SubmittedBy.Person.LastName);
                    });

                    message.Sorter.Compose("submittedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.SubmittedOn)
                            : query.OrderByDescending(x => x.SubmittedOn);
                    });

                    message.Sorter.Compose("paidBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.PaidBy.Person.FirstName)
                                .ThenBy(x => x.PaidBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.PaidBy.Person.LastName)
                                .ThenByDescending(x => x.PaidBy.Person.LastName);
                    });

                    message.Sorter.Compose("paidOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PaidOn)
                            : query.OrderByDescending(x => x.PaidOn);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
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
                                x.CreatedBy.Person.FirstName + " " +
                                x.CreatedBy.Person.LastName,
                            CreatedOn = x.CreatedOn,
                            SubmittedBy =
                                x.SubmittedBy.Person.FirstName + " " +
                                x.SubmittedBy.Person.LastName,
                            SubmittedOn = x.SubmittedOn,
                            PaidBy =
                                x.PaidBy.Person.FirstName + " " +
                                x.PaidBy.Person.LastName,
                            PaidOn = x.PaidOn,
                            TotalAmount = x.Total.Amount
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
