using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

// issue with groupby and count https://github.com/nhibernate/nhibernate-core/issues/1123

namespace AmpedBiz.Service.Orders
{
    // TODO: refactor this soon

    public class GetSalesReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.SalesReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                if (message.Filter == null)
                    message.Filter = new Filter();

                if (message.Filter.ContainsKey("date") == false)
                    message.Filter["date"] = DateTime.Now;

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<OrderItem>()
                        .Where(x => x.Order.Status == OrderStatus.Completed);

                    // compose filter
                    message.Filter.Compose<Guid>("productId", value =>
                    {
                        query = query.Where(x => x.Product.Id == value);
                    });

                    message.Filter.Compose<DateTime>("fromDate", value =>
                    {
                        query = query.Where(x => x.Order.CompletedOn >= value.StartOfDay());
                    });

                    message.Filter.Compose<DateTime>("toDate", value =>
                    {
                        query = query.Where(x => x.Order.CompletedOn <= value.EndOfDay());
                    });

                    message.Filter.Compose<DateTime>("date", value =>
                    {
                        var range = new
                        {
                            Start = value.Date,
                            End = value.Date.AddDays(1).AddSeconds(-1)
                        };

                        query = query.Where(x =>
                            x.Order.CompletedOn >= range.Start &&
                            x.Order.CompletedOn <= range.End
                        );
                    });

                    var headerQuery = query
                        .GroupBy(x => x.Product.Id)
                        .Select(x => new Dto.SalesReportPageItem()
                        {
                            ProductId = x.Key,
                            ProductName = x.Max(o => o.Product.Name)
                        });

                    // compose order
                    message.Sorter.Compose("productName", direction =>
                    {
                        headerQuery = direction == SortDirection.Ascending
                            ? headerQuery.OrderBy(x => x.ProductName)
                            : headerQuery.OrderByDescending(x => x.ProductName);
                    });

                    var headerCount = headerQuery.ToList().Count();

                    // https://github.com/nhibernate/nhibernate-core/issues/1123
                    //var headerCountFuture = query
                    //    .GroupBy(x => x.Product.Id)
                    //    .Select(group => group.Count())
                    //    .ToFutureValue(x => x.Sum());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(headerCount);
                    //message.Pager.RetrieveAll(headerCountFuture.Value);

                    var headerListFuture = headerQuery
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var detailsListFuture = query
                        .Fetch(x => x.Order)
                        .ThenFetch(x => x.Customer)
                        .ToFuture();

                    var idsFuture = headerQuery
                        .Select(x => x.ProductId)
                        .ToFuture();

                    var products = session.QueryOver<Product>()
                        .WhereRestrictionOn(x => x.Id).IsIn(idsFuture.ToArray())
                        .Fetch(x => x.Supplier).Eager
                        .Fetch(x => x.Inventories).Eager
                        .Fetch(x => x.UnitOfMeasures).Eager
                        .Fetch(x => x.UnitOfMeasures.First().UnitOfMeasure).Eager
                        .Future();

                    var headers = headerListFuture.ToList();
                    var details = detailsListFuture
                        .GroupBy(x => x.Product.Id)
                        .ToDictionary(x => x.Key, x => x.Select(o => o));

                    foreach (var header in headers)
                    {
                        var totalSoldItems = (Measure)null;
                        var totalSoldPrice = (Money)null;
                        var product = products.FirstOrDefault(x => x.Id == header.ProductId);

                        foreach (var detail in details[header.ProductId])
                        {
                            totalSoldItems += detail.QuantityStandardEquivalent;
                            totalSoldPrice += detail.TotalPrice;

                            header.Details.Add(new Dto.SalesReportPageDetailItem()
                            {
                                CustomerName = detail.Order.Customer.Name,
                                InvoiceNumber = detail.Order.InvoiceNumber,
                                SoldItems = detail.QuantityStandardEquivalent
                                    .BreakDown(product)
                                    .InterpretAsString(),
                                SoldPrice = detail.TotalPrice.ToString()
                            });
                        }

                        header.TotalSoldItems = totalSoldItems?
                            .BreakDown(product)
                            .InterpretAsString();

                        header.TotalSoldPrice = totalSoldPrice?.ToString();
                    }

                    response = new Response()
                    {
                        //Count = headerCountFuture.Value,
                        Count = headerCount,
                        Items = headers.ToList()
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
