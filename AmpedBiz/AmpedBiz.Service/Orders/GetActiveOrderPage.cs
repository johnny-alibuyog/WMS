using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetActiveOrderPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.OrderPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Order>().Where(x =>
                        x.Status != OrderStatus.Completed &&
                        x.Status != OrderStatus.Cancelled
                    );

                    // compose sort
                    message.Sorter.Compose("orderdOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.OrderedOn)
                            : query.OrderByDescending(x => x.OrderedOn);
                    });

                    message.Sorter.Compose("createdBy", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query
                                .OrderBy(x => x.CreatedBy.Person.FirstName)
                                .ThenBy(x => x.CreatedBy.Person.LastName)
                            : query
                                .OrderByDescending(x => x.CreatedBy.Person.FirstName)
                                .ThenByDescending(x => x.CreatedBy.Person.LastName);
                    });

                    message.Sorter.Compose("customer", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Customer)
                            : query.OrderByDescending(x => x.Customer);
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Status)
                            : query.OrderByDescending(x => x.Status);
                    });

                    message.Sorter.Compose("paidOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.PaidOn)
                            : query.OrderByDescending(x => x.PaidOn);
                    });

                    message.Sorter.Compose("taxAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Tax.Amount)
                            : query.OrderByDescending(x => x.Tax.Amount);
                    });

                    message.Sorter.Compose("shippingFeeAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ShippingFee.Amount)
                            : query.OrderByDescending(x => x.ShippingFee.Amount);
                    });

                    message.Sorter.Compose("subTotalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.SubTotal.Amount)
                            : query.OrderByDescending(x => x.SubTotal.Amount);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Total.Amount)
                            : query.OrderByDescending(x => x.Total.Amount);
                    });

                    var itemsFuture = query
                        .Select(x => new Dto.OrderPageItem()
                        {
                            Id = x.Id,
                            OrderedOn = x.OrderedOn.Value,
                            CreatedBy =
                                x.CreatedBy.Person.FirstName + " " +
                                x.CreatedBy.Person.LastName,
                            Customer = x.Customer.Name,
                            Status = x.Status.ToString(),
                            PaidOn = x.PaidOn.Value,
                            TaxAmount = x.Tax.Amount,
                            ShippingFeeAmount = x.ShippingFee.Amount,
                            SubTotalAmount = x.SubTotal.Amount,
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
