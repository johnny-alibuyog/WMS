using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Orders;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

// NOTE: issue with groupby and count https://github.com/nhibernate/nhibernate-core/issues/1123
// TODO: This has wrong pagination. Change soon
namespace AmpedBiz.Service.Products
{
	public class GetProductSalesReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.ProductSalesReportPageItem>
        {
            public string TotalSoldPrice { get; set; }
        }

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
                    var rawSales = (this.GetPointOfSaleSales(message, session))
                        .Union(this.GetOrderSales(message, session))
                        .ToList();

                    var query = rawSales
                        .GroupBy(x => x.Product)
                        .Select(x => new Dto.ProductSalesReportPageItem()
                        {
                            ProductId = x.Key.Id,
                            ProductName = x.Key.Name,
                            TotalSoldItems = x.SelectMany(o => o.Details)
                                .Sum(o => o.SoldItems)
                                .BreakDown(x.Key)
                                .InterpretAsString(),
                            TotalSoldPrice = x.SelectMany(o => o.Details)
                                .Sum(o => o.SoldPrice)
                                .ToStringOrDefault(),
                            Details = x.SelectMany(o => o.Details)
                                .Select(o => new Dto.ProductSalesReportPageDetailItem()
                                {
                                    SalesDate = o.SalesDate,
                                    CustomerName = o.CustomerName,
                                    InvoiceNumber = o.InvoiceNumber,
                                    SoldItems = o.SoldItems
                                        .BreakDown(x.Key)
                                        .InterpretAsString(),
                                    SoldPrice = o.SoldPrice
                                        .ToStringOrDefault()
                                })
                                .OrderBy(o => o.SalesDate)
                                .ToList()
                        });

					message.Sorter.Compose("productName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ProductName)
							: query.OrderByDescending(x => x.ProductName);
					});

					var totalItems = query.ToList();

					var totalCount = totalItems.Count();

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(totalCount);

					var pagedItems = totalItems
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToList();

                    response = new Response()
                    {
                        Count = totalCount,
                        Items = pagedItems,
                        TotalSoldPrice = rawSales
                            .SelectMany(x => x.Details)
                            .Sum(x => x.SoldPrice)
                            .ToStringOrDefault()
                    };

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}

			private List<RawHeader> GetPointOfSaleSales(Request message, ISession session)
			{
				var query = session.Query<PointOfSaleItem>();

				// compose filter
				message.Filter.Compose<Guid>("productId", value =>
				{
					query = query.Where(x => x.Product.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.PointOfSale.TenderedOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PointOfSale.TenderedOn <= value.EndOfDay());
				});

				message.Filter.Compose<DateTime>("date", value =>
				{
					var range = new
					{
						Start = value.Date.StartOfDay(),
						End = value.Date.EndOfDay()
					};

					query = query.Where(x =>
						x.PointOfSale.TenderedOn >= range.Start &&
						x.PointOfSale.TenderedOn <= range.End
					);
				});

				var headerQuery = query
					.GroupBy(x => x.Product.Id)
					.Select(x => new
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

                // https://github.com/nhibernate/nhibernate-core/issues/1123
                var headerCount = query.ToList().Count();
                //var headerCount = query.GroupByCount(x => x.Product.Id);

                if (message.Pager.IsPaged() != true)
					message.Pager.RetrieveAll(headerCount);
					//message.Pager.RetrieveAll(headerCount.Value);

				var headerListFuture = headerQuery
					.Skip(message.Pager.SkipCount)
					.Take(message.Pager.Size)
					.ToFuture();

				var detailsListFuture = query
					.Fetch(x => x.PointOfSale)
					.ThenFetch(x => x.Customer)
					.ToFuture();

				var idsFuture = headerQuery
					.Select(x => x.ProductId)
                    .Distinct()
					.ToFuture();

				var products = session.QueryOver<Product>()
					.WhereRestrictionOn(x => x.Id).IsIn(idsFuture.ToArray())
					.Fetch(x => x.Inventories).Eager
					.Fetch(x => x.UnitOfMeasures).Eager
					.Fetch(x => x.UnitOfMeasures.First().UnitOfMeasure).Eager
					.Future();

				var headers = headerListFuture.ToList();

				var details = detailsListFuture
					.GroupBy(x => x.Product.Id)
					.ToDictionary(x => x.Key, x => x.Select(o => o));

                var rawHeaders = new List<RawHeader>();

				foreach (var header in headers)
				{
                    var rawHeader = new RawHeader();

                    rawHeader.Product = products.FirstOrDefault(x => x.Id == header.ProductId);

					foreach (var detail in details[header.ProductId])
					{
                        rawHeader.Details.Add(new RawDetail()
						{
                            SalesDate = detail.PointOfSale.TenderedOn,
							CustomerName = detail.PointOfSale.Customer.Name,
							InvoiceNumber = detail.PointOfSale.InvoiceNumber,
							SoldItems = detail.QuantityStandardEquivalent,
							SoldPrice = detail.TotalPrice
						});
					}

                    rawHeaders.Add(rawHeader);
				}

				return rawHeaders;
			}

			private List<RawHeader> GetOrderSales(Request message, ISession session)
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
						Start = value.Date.StartOfDay(),
						End = value.Date.EndOfDay()
					};

					query = query.Where(x =>
						x.Order.CompletedOn >= range.Start &&
						x.Order.CompletedOn <= range.End
					);
				});

				var headerQuery = query
					.GroupBy(x => x.Product.Id)
					.Select(x => new
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

                // https://github.com/nhibernate/nhibernate-core/issues/1123
                var headerCount = query.ToList().Count(); //.GroupByCount(x => x.Product.Id);
				//var headerCount = query.GroupByCount(x => x.Product.Id);

                if (message.Pager.IsPaged() != true)
					message.Pager.RetrieveAll(headerCount);
					//message.Pager.RetrieveAll(headerCount.Value);

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
					.Fetch(x => x.Inventories).Eager
					.Fetch(x => x.UnitOfMeasures).Eager
					.Fetch(x => x.UnitOfMeasures.First().UnitOfMeasure).Eager
					.Future();

				var headers = headerListFuture.ToList();

				var details = detailsListFuture
					.GroupBy(x => x.Product.Id)
					.ToDictionary(x => x.Key, x => x.Select(o => o));

                var rawHeaders = new List<RawHeader>();

				foreach (var header in headers)
				{
                    var rawHeader = new RawHeader();

                    rawHeader.Product = products.FirstOrDefault(x => x.Id == header.ProductId);

					foreach (var detail in details[header.ProductId])
					{
						rawHeader.Details.Add(new RawDetail()
						{
                            SalesDate = detail.Order.CompletedOn,
							CustomerName = detail.Order.Customer.Name,
							InvoiceNumber = detail.Order.InvoiceNumber,
							SoldItems = detail.QuantityStandardEquivalent,
							SoldPrice = detail.TotalPrice
						});
					}
				}

				return rawHeaders;
			}
		}

        public class RawHeader
        {
            public Product Product { get; set; }

            public List<RawDetail> Details { get; set; } = new List<RawDetail>();
        }

        public class RawDetail
        {
            public DateTime? SalesDate { get; set; }

            public string CustomerName { get; set; }

            public string InvoiceNumber { get; set; }

            public Measure SoldItems { get; set; }

            public Money SoldPrice { get; set; }
        }
    }
}
