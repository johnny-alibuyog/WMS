using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Orders;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
	public class GetCustomerOrderDeliveryReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.CustomerOrderDeliveryReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Order>().Where(x => 
                        x.ShippedBy != null && 
                        x.ShippedOn != null &&
                        x.Status != OrderStatus.Cancelled
                    );

                    // compose filters
                    message.Filter.Compose<Guid>("branchId", value =>
                    {
                        query = query.Where(x => x.Branch.Id == value);
                    });

                    message.Filter.Compose<DateTime>("fromDate", value =>
                    {
                        query = query.Where(x => x.ShippedOn >= value.StartOfDay());
                    });

                    message.Filter.Compose<DateTime>("toDate", value =>
                    {
                        query = query.Where(x => x.ShippedOn <= value.EndOfDay());
                    });

                    // compose order
                    message.Sorter.Compose("shipedOn", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ShippedOn)
                            : query.OrderByDescending(x => x.ShippedOn);
                    });

                    message.Sorter.Compose("invoiceNumber", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.InvoiceNumber)
                            : query.OrderByDescending(x => x.InvoiceNumber);
                    });

                    message.Sorter.Compose("branchName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Branch.Name)
                            : query.OrderByDescending(x => x.Branch.Name);
                    });

                    message.Sorter.Compose("customerName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Customer.Name)
                            : query.OrderByDescending(x => x.Customer.Name);
                    });

                    message.Sorter.Compose("pricingName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Pricing.Name)
                            : query.OrderByDescending(x => x.Pricing.Name);
                    });

                    message.Sorter.Compose("discountAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Discount.Amount)
                            : query.OrderByDescending(x => x.Discount.Amount);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Total.Amount)
                            : query.OrderByDescending(x => x.Total.Amount);
                    });

                    message.Sorter.Compose("subTotalAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.SubTotal.Amount)
                            : query.OrderByDescending(x => x.SubTotal.Amount);
                    });

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new Dto.CustomerOrderDeliveryReportPageItem()
                        {
                            OrderId = x.Id,
                            ShippedOn = x.ShippedOn,
                            BranchName = x.Branch.Name,
                            InvoiceNumber = x.InvoiceNumber,
                            CustomerName = x.Customer.Name,
                            PricingName = x.Pricing.Name,
                            DiscountAmount = x.Discount.Amount,
                            TotalAmount = x.Total.Amount,
                            SubTotalAmount = x.SubTotal.Amount
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

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
