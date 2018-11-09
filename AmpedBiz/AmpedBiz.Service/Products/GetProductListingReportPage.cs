using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProductListingReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.ProductListingReportPageItem> { }

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
					//session.DisableBranchFilter(); //TODO: include branch to Pricing

					var query = session.Query<ProductUnitOfMeasure>()
						.Where(x => x.IsStandard || x.IsDefault);

					// compose filter
					//message.Filter.Compose<Guid>("branchId", value =>
					//{
					//    query = query.Where(x => x.Order.Branch.Id == value);
					//});

					message.Filter.Compose<Guid>("supplierId", value =>
					{
						query = query.Where(x => x.Product.Supplier.Id == value);
					});

					message.Filter.Compose<string>("categoryId", value =>
					{
						query = query.Where(x => x.Product.Category.Id == value);
					});

					message.Filter.Compose<Guid>("productId", value =>
					{
						query = query.Where(x => x.Product.Id == value);
					});

					// compose sort order
					//message.Sorter.Compose("branchName", direction =>
					//{
					//    query = direction == SortDirection.Ascending
					//        ? query.OrderBy(x => x.Branch.Name)
					//        : query.OrderByDescending(x => x.Branch.Name);
					//});

					message.Sorter.Compose("supplierName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Product.Supplier.Name)
							: query.OrderByDescending(x => x.Product.Supplier.Name);
					});

					message.Sorter.Compose("categoryName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Product.Category.Name)
							: query.OrderByDescending(x => x.Product.Category.Name);
					});

					message.Sorter.Compose("productName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Product.Name)
							: query.OrderByDescending(x => x.Product.Name);
					});

					message.Sorter.Compose("unitOfMeasure", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.UnitOfMeasure.Name)
							: query.OrderByDescending(x => x.UnitOfMeasure.Name);
					});

					var countFuture = query
						.ToFutureValue(x => x.Count());

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(countFuture.Value);

					var idsFuture = query
						.Select(x => new
						{
							ProductUnitOfMeasureId = x.Id,
							ProductId = x.Product.Id,
							UnitOfMeasureId = x.UnitOfMeasure.Id,
						})
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToFuture();

					var ids = new
					{
						ProductUnitOfMeasureIds = idsFuture.Select(x => x.ProductUnitOfMeasureId).Distinct().ToList(),
						ProductIds = idsFuture.Select(x => x.ProductId).Distinct().ToList(),
						UnitOfMeasureIds = idsFuture.Select(x => x.UnitOfMeasureId).Distinct().ToList()
					};

					var branch = session.Get<Branch>(this.Context.BranchId);

					var products = session.Query<Product>()
						.Where(x => ids.ProductIds.Contains(x.Id))
						.Fetch(x => x.Supplier)
						.Fetch(x => x.Category)
						.FetchMany(x => x.UnitOfMeasures)
						.ThenFetch(x => x.UnitOfMeasure)
						.ToFuture();

					var productUnitOfMeasures = session.Query<ProductUnitOfMeasure>()
						.Where(x => ids.ProductUnitOfMeasureIds.Contains(x.Id))
						.Fetch(x => x.Prices)
						.ToFuture();

					var unitOfMeasures = session.Query<UnitOfMeasure>()
						.Where(x => ids.UnitOfMeasureIds.Contains(x.Id))
						.ToFuture();

					var inventories = session.Query<Inventory>()
						.Where(x => ids.ProductIds.Contains(x.Product.Id))
						.Fetch(x => x.Branch)
						.Fetch(x => x.OnHand)
						.Fetch(x => x.Available)
						.ToFuture();

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(countFuture.Value);

					var Hydrate = new Func<Guid, Guid, string, Dto.ProductListingReportPageItem>((productUnitOfMeasureId, productId, unitOfMeasureId) =>
					{
						var lookup = new
						{

							ProductUnitOfMeasure = productUnitOfMeasures.Single(x => x.Id == productUnitOfMeasureId),
							Product = products.Single(x => x.Id == productId),
							UnitOfMeasure = unitOfMeasures.Single(x => x.Id == unitOfMeasureId),
							Inventory = inventories.Single(x => x.Product.Id == productId)
						};

						var item = new Dto.ProductListingReportPageItem()
						{
							BranchName = branch.Name,
							ProductName = lookup.Product.Name,
							SupplierName = lookup.Product.Supplier.Name,
							CategoryName = lookup.Product.Category.Name,
							QuantityUnit = lookup.UnitOfMeasure.Name,
							AvailableValue = lookup.Inventory.Available.TakePartValue(
								product: lookup.Product,
								part: lookup.UnitOfMeasure
							),
							OnHandValue = lookup.Inventory.OnHand.TakePartValue(
								product: lookup.Product,
								part: lookup.UnitOfMeasure
							),
							BasePriceAmount = lookup.ProductUnitOfMeasure.Prices.Base()?.Amount,
							RetailPriceAmount = lookup.ProductUnitOfMeasure.Prices.Retail()?.Amount,
							SuggestedRetailPriceAmount = lookup.ProductUnitOfMeasure.Prices.SuggestedRetail()?.Amount,
							WholesalePriceAmount = lookup.ProductUnitOfMeasure.Prices.Wholesale()?.Amount

						};
						return item;
					});

					response = new Response()
					{
						Count = countFuture.Value,
						Items = idsFuture
							.Select(x => Hydrate(
								arg1: x.ProductUnitOfMeasureId,
								arg2: x.ProductId,
								arg3: x.UnitOfMeasureId
							))
							.Distinct()
							.ToList()
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}

	//public class GetProductListingReportPage
	//{
	//    public class Request : PageRequest, IRequest<Response> { }

	//    public class Response : PageResponse<Dto.ProductListingReportPageItem> { }

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
	//                var query = session.Query<ProductUnitOfMeasure>();

	//                //// compose filter
	//                //message.Filter.Compose<Guid>("branchId", value =>
	//                //{
	//                //    query = query.Where(x => x.Order.Branch.Id == value);
	//                //});

	//                //message.Filter.Compose<Guid>("supplierId", value =>
	//                //{
	//                //    query = query.Where(x => x.Product.Supplier.Id == value);
	//                //});

	//                //message.Filter.Compose<string>("categoryId", value =>
	//                //{
	//                //    query = query.Where(x => x.Product.Category.Id == value);
	//                //});

	//                //message.Filter.Compose<Guid>("productId", value =>
	//                //{
	//                //    query = query.Where(x => x.Product.Id == value);
	//                //});

	//                //message.Filter.Compose<DateTime>("fromDate", value =>
	//                //{
	//                //    query = query.Where(x => x.Order.CompletedOn >= value.StartOfDay());
	//                //});

	//                //message.Filter.Compose<DateTime>("toDate", value =>
	//                //{
	//                //    query = query.Where(x => x.Order.CompletedOn <= value.EndOfDay());
	//                //});

	//                var groupedQuery = query
	//                    .GroupBy(x => new
	//                    {
	//                        //BranchName = x.Product.Inventory.Branch.Name,
	//                        SupplierName = x.Product.Supplier.Name,
	//                        CategoryName = x.Product.Category.Name,
	//                        ProductName = x.Product.Name,
	//                        QuantityUnitName = x.UnitOfMeasure.Name,
	//                        //UnitPriceAmount = x.UnitPrice.Amount,
	//                        //DiscountRate = x.DiscountRate,
	//                    })
	//                    .Select(x => new Dto.ProductListingReportPageItem()
	//                    {
	//                        //BranchName = x.Key.BranchName,
	//                        SupplierName = x.Key.SupplierName,
	//                        CategoryName = x.Key.CategoryName,
	//                        ProductName = x.Key.ProductName,
	//                        QuantityUnit = x.Key.QuantityUnitName,
	//                    });


	//                //// compose order
	//                //message.Sorter.Compose("completedOn", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery
	//                //            .OrderBy(x => x.Year)
	//                //            .ThenBy(x => x.Month)
	//                //            .ThenBy(x => x.Day)
	//                //        : groupedQuery
	//                //            .OrderByDescending(x => x.Year)
	//                //            .ThenByDescending(x => x.Month)
	//                //            .ThenByDescending(x => x.Day);
	//                //});

	//                //message.Sorter.Compose("branchName", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.BranchName)
	//                //        : groupedQuery.OrderByDescending(x => x.BranchName);
	//                //});

	//                //message.Sorter.Compose("supplierName", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.SupplierName)
	//                //        : groupedQuery.OrderByDescending(x => x.SupplierName);
	//                //});

	//                //message.Sorter.Compose("categoryName", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.CategoryName)
	//                //        : groupedQuery.OrderByDescending(x => x.CategoryName);
	//                //});

	//                //message.Sorter.Compose("productName", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.ProductName)
	//                //        : groupedQuery.OrderByDescending(x => x.ProductName);
	//                //});

	//                //message.Sorter.Compose("quantityUnit", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.QuantityUnit)
	//                //        : groupedQuery.OrderByDescending(x => x.QuantityUnit);
	//                //});

	//                //message.Sorter.Compose("quantityValue", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.QuantityValue)
	//                //        : groupedQuery.OrderByDescending(x => x.QuantityValue);
	//                //});

	//                //message.Sorter.Compose("unitPriceAmount", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.UnitPriceAmount)
	//                //        : groupedQuery.OrderByDescending(x => x.UnitPriceAmount);
	//                //});

	//                //message.Sorter.Compose("discountAmount", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.DiscountAmount)
	//                //        : groupedQuery.OrderByDescending(x => x.DiscountAmount);
	//                //});

	//                //message.Sorter.Compose("extendedPriceAmount", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.ExtendedPriceAmount)
	//                //        : groupedQuery.OrderByDescending(x => x.ExtendedPriceAmount);
	//                //});

	//                //message.Sorter.Compose("totalPriceAmount", direction =>
	//                //{
	//                //    groupedQuery = direction == SortDirection.Ascending
	//                //        ? groupedQuery.OrderBy(x => x.TotalPriceAmount)
	//                //        : groupedQuery.OrderByDescending(x => x.TotalPriceAmount);
	//                //});

	//                var countFuture = groupedQuery
	//                    .Select(x => x.ProductName)
	//                    .ToFuture();

	//                if (message.Pager.IsPaged() != true)
	//                    message.Pager.RetrieveAll(countFuture.Count());

	//                var itemsFuture = groupedQuery
	//                    .Skip(message.Pager.SkipCount)
	//                    .Take(message.Pager.Size)
	//                    .ToFuture();

	//                response = new Response()
	//                {
	//                    Count = countFuture.Count(),
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
