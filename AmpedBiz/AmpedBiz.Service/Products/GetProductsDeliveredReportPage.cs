using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Orders;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProductsDeliveredReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.ProductsDeliveredReportPageItem> { }

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
					var query = session.Query<OrderItem>().Where(x =>
						x.Order.Status == OrderStatus.Shipped ||
						x.Order.Status == OrderStatus.Completed
					);

					// compose filter
					message.Filter.Compose<Guid>("branchId", value =>
					{
						query = query.Where(x => x.Order.Branch.Id == value);
					});

					message.Filter.Compose<string>("categoryId", value =>
					{
						query = query.Where(x => x.Product.Category.Id == value);
					});

					message.Filter.Compose<Guid>("productId", value =>
					{
						query = query.Where(x => x.Product.Id == value);
					});

					message.Filter.Compose<DateTime>("fromDate", value =>
					{
						query = query.Where(x => x.Order.ShippedOn >= value.StartOfDay());
					});

					message.Filter.Compose<DateTime>("toDate", value =>
					{
						query = query.Where(x => x.Order.ShippedOn <= value.EndOfDay());
					});

					var groupedQuery = query
						.GroupBy(x => new
						{
							BranchName = x.Order.Branch.Name,
							CategoryName = x.Product.Category.Name,
							ProductName = x.Product.Name,
							QuantityUnitName = x.Quantity.Unit.Name,
							UnitPriceAmount = x.UnitPrice.Amount,
						})
						.Select(x => new Dto.ProductsDeliveredReportPageItem()
						{
							BranchName = x.Key.BranchName,
							CategoryName = x.Key.CategoryName,
							ProductName = x.Key.ProductName,
							UnitPriceAmount = x.Key.UnitPriceAmount,
							QuantityUnit = x.Key.QuantityUnitName,
							QuantityValue = x.Sum(o => o.Quantity.Value),
							DiscountAmount = x.Sum(o => o.Discount.Amount),
							ExtendedPriceAmount = x.Sum(o => o.ExtendedPrice.Amount),
							TotalPriceAmount = x.Sum(o => o.TotalPrice.Amount)
						});


					// compose order
					message.Sorter.Compose("branchName", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.BranchName)
							: groupedQuery.OrderByDescending(x => x.BranchName);
					});

					message.Sorter.Compose("categoryName", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.CategoryName)
							: groupedQuery.OrderByDescending(x => x.CategoryName);
					});

					message.Sorter.Compose("productName", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.ProductName)
							: groupedQuery.OrderByDescending(x => x.ProductName);
					});

					message.Sorter.Compose("quantityUnit", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.QuantityUnit)
							: groupedQuery.OrderByDescending(x => x.QuantityUnit);
					});

					message.Sorter.Compose("quantityValue", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.QuantityValue)
							: groupedQuery.OrderByDescending(x => x.QuantityValue);
					});

					message.Sorter.Compose("unitPriceAmount", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.UnitPriceAmount)
							: groupedQuery.OrderByDescending(x => x.UnitPriceAmount);
					});

					message.Sorter.Compose("discountAmount", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.DiscountAmount)
							: groupedQuery.OrderByDescending(x => x.DiscountAmount);
					});

					message.Sorter.Compose("extendedPriceAmount", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.ExtendedPriceAmount)
							: groupedQuery.OrderByDescending(x => x.ExtendedPriceAmount);
					});

					message.Sorter.Compose("totalPriceAmount", direction =>
					{
						groupedQuery = direction == SortDirection.Ascending
							? groupedQuery.OrderBy(x => x.TotalPriceAmount)
							: groupedQuery.OrderByDescending(x => x.TotalPriceAmount);
					});

					//var countFuture = groupedQuery
					//    .Select(x => new { BranchName = x.BranchName })
					//    .ToFuture();

					// TODO: this is not performant, this is just a work around on groupby count issue of nhibernate. find a solution soon
					var totalItems = groupedQuery.ToList();

					var count = totalItems.Count;

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(count);

					var items = groupedQuery
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
}

// GROUP BY WITH PROJECTION REFERENCES: 
// http://www.andrewwhitaker.com/blog/2014/03/22/queryover-series-part-3-selecting-and-transforming/
// http://www.andrewwhitaker.com/blog/2014/08/07/queryover-series-part-6-query-building-techniques/

//public class GetProductsDeliveredReportPage
//{
//    public class Request : PageRequest, IRequest<Response> { }

//    public class Response : PageResponse<Dto.ProductsDeliveredReportPageItem> { }

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
//                var dto = default(Dto.ProductsDeliveredReportPageItem);

//                var orderItem = default(OrderItem);
//                var order = default(Core.Entities.Order);
//                var product = default(Product);
//                var branch = default(Branch);
//                var supplier = default(Supplier);
//                var category = default(ProductCategory);
//                var quantity = default(Measure);
//                var quantityUnit = default(UnitOfMeasure);
//                var extendedPrice = default(Money);
//                var discount = default(Money);
//                var totalPrice = default(Money);

//                var query = session.QueryOver(() => orderItem)
//                    .JoinAlias(() => orderItem.Order, () => order)
//                    .JoinAlias(() => orderItem.Product, () => product)
//                    .JoinAlias(() => orderItem.Quantity, () => quantity)
//                    .JoinAlias(() => orderItem.ExtendedPrice, () => extendedPrice)
//                    .JoinAlias(() => orderItem.Discount, () => discount)
//                    .JoinAlias(() => orderItem.TotalPrice, () => totalPrice)
//                    .JoinAlias(() => order.Branch, () => branch)
//                    .JoinAlias(() => product.Supplier, () => supplier)
//                    .JoinAlias(() => product.Category, () => category)
//                    .JoinAlias(() => quantity.Unit, () => quantityUnit)
//                    .SelectList(fields => fields
//                        .SelectGroup(() => order.CompletedOn.Value.Year).WithAlias(() => dto.Year)
//                        .SelectGroup(() => order.CompletedOn.Value.Month).WithAlias(() => dto.Month)
//                        .SelectGroup(() => order.CompletedOn.Value.Day).WithAlias(() => dto.Day)
//                        .SelectGroup(() => branch.Name).WithAlias(() => dto.BranchName)
//                        .SelectGroup(() => supplier.Name).WithAlias(() => dto.SupplierName)
//                        .SelectGroup(() => category.Name).WithAlias(() => dto.CategoryName)
//                        .SelectGroup(() => product.Name).WithAlias(() => dto.ProductName)
//                        .SelectGroup(() => quantityUnit.Id).WithAlias(() => dto.QuantityUnit)
//                        .SelectSum(() => extendedPrice.Amount).WithAlias(() => dto.ExtendedPriceAmount)
//                        .SelectSum(() => quantity.Value).WithAlias(() => dto.QuantityValue)
//                        .SelectSum(() => totalPrice.Amount).WithAlias(() => dto.TotalPriceAmount)
//                    );

//                //var query = session.Query<OrderItem>()
//                //    .Where(x => x.Status == OrderStatus.Completed);

//                //// compose filter
//                //message.Filter.Compose<Guid>("branchId", value =>
//                //{
//                //    query = query.Where(x => x.Branch.Id == value);
//                //});

//                //message.Filter.Compose<Guid>("customerId", value =>
//                //{
//                //    query = query.Where(x => x.Customer.Id == value);
//                //});

//                //message.Filter.Compose<DateTime>("fromDate", value =>
//                //{
//                //    query = query.Where(x => x.CompletedOn >= value.StartOfDay());
//                //});

//                //message.Filter.Compose<DateTime>("toDate", value =>
//                //{
//                //    query = query.Where(x => x.CompletedOn <= value.EndOfDay());
//                //});

//                //// compose order
//                //message.Sorter.Compose("completedOn", direction =>
//                //{
//                //    query = direction == SortDirection.Ascending
//                //        ? query.OrderBy(x => x.CompletedOn)
//                //        : query.OrderByDescending(x => x.CompletedOn);
//                //});

//                //message.Sorter.Compose("branchName", direction =>
//                //{
//                //    query = direction == SortDirection.Ascending
//                //        ? query.OrderBy(x => x.Branch.Name)
//                //        : query.OrderByDescending(x => x.Branch.Name);
//                //});

//                //message.Sorter.Compose("customerName", direction =>
//                //{
//                //    query = direction == SortDirection.Ascending
//                //        ? query.OrderBy(x => x.Customer.Name)
//                //        : query.OrderByDescending(x => x.Customer.Name);
//                //});

//                //message.Sorter.Compose("invoice", direction =>
//                //{
//                //    query = direction == SortDirection.Ascending
//                //        ? query.OrderBy(x => x.InvoiceNumber)
//                //        : query.OrderByDescending(x => x.InvoiceNumber);
//                //});

//                //message.Sorter.Compose("totalAmount", direction =>
//                //{
//                //    query = direction == SortDirection.Ascending
//                //        ? query.OrderBy(x => x.Total.Amount)
//                //        : query.OrderByDescending(x => x.Total.Amount);
//                //});

//                //message.Sorter.Compose("balanceAmount", direction =>
//                //{
//                //    query = direction == SortDirection.Ascending
//                //        ? query.OrderBy(x => x.Total.Amount - x.Paid.Amount)
//                //        : query.OrderByDescending(x => x.Total.Amount - x.Paid.Amount);
//                //});

//                //var countFuture = query
//                //    .ToFutureValue(x => x.Count());

//                //if (message.Pager.IsPaged() != true)
//                //    message.Pager.RetrieveAll(countFuture.Value);

//                //var itemsFuture = query
//                //    .Select(x => new Dto.CustomerSalesReportPageItem()
//                //    {
//                //        CompletedOn = x.CompletedOn,
//                //        BranchName = x.Branch.Name,
//                //        CustomerName = x.Customer.Name,
//                //        InvoiceNumber = x.InvoiceNumber,
//                //        TotalAmount = x.Total.Amount,
//                //        PaidAmount = x.Paid.Amount,
//                //    })
//                //    .Skip(message.Pager.SkipCount)
//                //    .Take(message.Pager.Size)
//                //    .ToFuture();

//                //response = new Response()
//                //{
//                //    Count = countFuture.Value,
//                //    Items = itemsFuture.ToList()
//                //};

//                //if (message.Pager.IsPaged() != true)
//                //    message.Pager.RetrieveAll(countFuture.Value);

//                var itemsFuture = (message.Pager.IsPaged())
//                    ? query
//                        .TransformUsing(Transformers.AliasToBean<Dto.ProductsDeliveredReportPageItem>())
//                        .Skip(message.Pager.SkipCount)
//                        .Take(message.Pager.Size)
//                        .Future<Dto.ProductsDeliveredReportPageItem>()
//                    : query
//                        .TransformUsing(Transformers.AliasToBean<Dto.ProductsDeliveredReportPageItem>())
//                        .Future<Dto.ProductsDeliveredReportPageItem>();

//                //var countFuture = query
//                //    .Select(Projections.RowCount())
//                //    .FutureValue<int>();

//                response = new Response()
//                {
//                    Count = 0,
//                    Items = itemsFuture.ToList()
//                };

//                transaction.Commit();
//            }

//            return response;
//        }
//    }
//}
