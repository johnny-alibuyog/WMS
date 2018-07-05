using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
    public class GetCustomerSalesReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.CustomerSalesReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                if (message.Filter == null)
                    message.Filter = new Filter();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<OrderPayment>();

                    // compose filter
                    message.Filter.Compose<Guid>("branchId", value =>
                    {
                        query = query.Where(x => x.Order.Branch.Id == value);
                    });

                    message.Filter.Compose<Guid>("customerId", value =>
                    {
                        query = query.Where(x => x.Order.Customer.Id == value);
                    });

                    message.Filter.Compose<DateTime>("fromDate", value =>
                    {
                        query = query.Where(x => x.PaidOn >= value.StartOfDay());
                    });

                    message.Filter.Compose<DateTime>("toDate", value =>
                    {
                        query = query.Where(x => x.PaidOn <= value.EndOfDay());
                    });

                    var groupedQuery = query
                        .GroupBy(x => new
                        {
                            BranchName = x.Order.Branch.Name,
                            CustomerName = x.Order.Customer.Name,
                            InvoiceNumber = x.Order.InvoiceNumber,
                        })
                        .Select(x => new Dto.CustomerSalesReportPageItem()
                        {
                            PaidOn = x.Max(o => o.PaidOn),
                            BranchName = x.Key.BranchName,
                            CustomerName = x.Key.CustomerName,
                            InvoiceNumber = x.Key.InvoiceNumber,
                            Status = x.Min(o => o.Order.Status).ToString(),
                            TotalAmount = x.Max(o => o.Order.Total.Amount),
                            PaidAmount = x.Sum(o => o.Payment.Amount)
                        });

                    // compose order
                    message.Sorter.Compose("paidOn", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.PaidOn)
                            : groupedQuery.OrderByDescending(x => x.PaidOn);
                    });

                    message.Sorter.Compose("branchName", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.BranchName)
                            : groupedQuery.OrderByDescending(x => x.BranchName);
                    });

                    message.Sorter.Compose("customerName", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.CustomerName)
                            : groupedQuery.OrderByDescending(x => x.CustomerName);
                    });

                    message.Sorter.Compose("invoice", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.InvoiceNumber)
                            : groupedQuery.OrderByDescending(x => x.InvoiceNumber);
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.Status)
                            : groupedQuery.OrderByDescending(x => x.Status);
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.TotalAmount)
                            : groupedQuery.OrderByDescending(x => x.TotalAmount);
                    });

                    message.Sorter.Compose("balanceAmount", direction =>
                    {
                        groupedQuery = direction == SortDirection.Ascending
                            ? groupedQuery.OrderBy(x => x.TotalAmount - x.PaidAmount)
                            : groupedQuery.OrderByDescending(x => x.TotalAmount - x.PaidAmount);
                    });

                    // TODO: this is not performant, this is just a work around on groupby count issue of nhibernate. find a solution soon
                    var totalItems = groupedQuery.ToList();

                    var count = totalItems.Count;

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(count);

                    var items = totalItems
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToList();

                    response = new Response()
                    {
                        Count = count,
                        Items = items
                    };

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }

    //public class GetCustomerSalesReportPage1
    //{
    //    public class Request : PageRequest, IRequest<Response> { }

    //    public class Response : PageResponse<Dto.CustomerSalesReportPageItem> { }

    //    public class Handler : RequestHandlerBase<Request, Response>
    //    {
    //        public override Response Execute(Request message)
    //        {
    //            var response = new Response();

    //            if (message.Filter == null)
    //                message.Filter = new Filter();

    //            using (var session = SessionFactory.RetrieveSharedSession(Context))
    //            using (var transaction = session.BeginTransaction())
    //            {
    //                var query = session.Query<Order>()
    //                    .Where(x => x.Status == OrderStatus.Completed);

    //                // compose filter
    //                message.Filter.Compose<Guid>("branchId", value =>
    //                {
    //                    query = query.Where(x => x.Branch.Id == value);
    //                });

    //                message.Filter.Compose<Guid>("customerId", value =>
    //                {
    //                    query = query.Where(x => x.Customer.Id == value);
    //                });

    //                message.Filter.Compose<DateTime>("fromDate", value =>
    //                {
    //                    query = query.Where(x => x.CompletedOn >= value.StartOfDay());
    //                });

    //                message.Filter.Compose<DateTime>("toDate", value =>
    //                {
    //                    query = query.Where(x => x.CompletedOn <= value.EndOfDay());
    //                });

    //                // compose order
    //                message.Sorter.Compose("completedOn", direction =>
    //                {
    //                    query = direction == SortDirection.Ascending
    //                        ? query.OrderBy(x => x.CompletedOn)
    //                        : query.OrderByDescending(x => x.CompletedOn);
    //                });

    //                message.Sorter.Compose("branchName", direction =>
    //                {
    //                    query = direction == SortDirection.Ascending
    //                        ? query.OrderBy(x => x.Branch.Name)
    //                        : query.OrderByDescending(x => x.Branch.Name);
    //                });

    //                message.Sorter.Compose("customerName", direction =>
    //                {
    //                    query = direction == SortDirection.Ascending
    //                        ? query.OrderBy(x => x.Customer.Name)
    //                        : query.OrderByDescending(x => x.Customer.Name);
    //                });

    //                message.Sorter.Compose("invoice", direction =>
    //                {
    //                    query = direction == SortDirection.Ascending
    //                        ? query.OrderBy(x => x.InvoiceNumber)
    //                        : query.OrderByDescending(x => x.InvoiceNumber);
    //                });

    //                message.Sorter.Compose("totalAmount", direction =>
    //                {
    //                    query = direction == SortDirection.Ascending
    //                        ? query.OrderBy(x => x.Total.Amount)
    //                        : query.OrderByDescending(x => x.Total.Amount);
    //                });

    //                message.Sorter.Compose("balanceAmount", direction =>
    //                {
    //                    query = direction == SortDirection.Ascending
    //                        ? query.OrderBy(x => x.Total.Amount - x.Paid.Amount)
    //                        : query.OrderByDescending(x => x.Total.Amount - x.Paid.Amount);
    //                });

    //                var countFuture = query
    //                    .ToFutureValue(x => x.Count());

    //                if (message.Pager.IsPaged() != true)
    //                    message.Pager.RetrieveAll(countFuture.Value);

    //                var itemsFuture = query
    //                    .Select(x => new Dto.CustomerSalesReportPageItem()
    //                    {
    //                        PaidOn = x.CompletedOn,
    //                        BranchName = x.Branch.Name,
    //                        CustomerName = x.Customer.Name,
    //                        InvoiceNumber = x.InvoiceNumber,
    //                        TotalAmount = x.Total.Amount,
    //                        PaidAmount = x.Paid.Amount,
    //                    })
    //                    .Skip(message.Pager.SkipCount)
    //                    .Take(message.Pager.Size)
    //                    .ToFuture();

    //                response = new Response()
    //                {
    //                    Count = countFuture.Value,
    //                    Items = itemsFuture.ToList()
    //                };

    //                transaction.Commit();

    //                SessionFactory.ReleaseSharedSession();
    //            }

    //            return response;
    //        }
    //    }
    //}
}
