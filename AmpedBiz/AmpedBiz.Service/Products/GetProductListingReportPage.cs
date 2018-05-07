using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
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
                    var query = session.Query<ProductUnitOfMeasure>();

                    //// compose filter
                    //message.Filter.Compose<Guid>("branchId", value =>
                    //{
                    //    query = query.Where(x => x.Order.Branch.Id == value);
                    //});

                    //message.Filter.Compose<Guid>("supplierId", value =>
                    //{
                    //    query = query.Where(x => x.Product.Supplier.Id == value);
                    //});

                    //message.Filter.Compose<string>("categoryId", value =>
                    //{
                    //    query = query.Where(x => x.Product.Category.Id == value);
                    //});

                    //message.Filter.Compose<Guid>("productId", value =>
                    //{
                    //    query = query.Where(x => x.Product.Id == value);
                    //});

                    //message.Filter.Compose<DateTime>("fromDate", value =>
                    //{
                    //    query = query.Where(x => x.Order.CompletedOn >= value.StartOfDay());
                    //});

                    //message.Filter.Compose<DateTime>("toDate", value =>
                    //{
                    //    query = query.Where(x => x.Order.CompletedOn <= value.EndOfDay());
                    //});

                    var groupedQuery = query
                        .GroupBy(x => new
                        {
                            //BranchName = x.Product.Inventory.Branch.Name,
                            SupplierName = x.Product.Supplier.Name,
                            CategoryName = x.Product.Category.Name,
                            ProductName = x.Product.Name,
                            QuantityUnitName = x.UnitOfMeasure.Name,
                            //UnitPriceAmount = x.UnitPrice.Amount,
                            //DiscountRate = x.DiscountRate,
                        })
                        .Select(x => new Dto.ProductListingReportPageItem()
                        {
                            //BranchName = x.Key.BranchName,
                            SupplierName = x.Key.SupplierName,
                            CategoryName = x.Key.CategoryName,
                            ProductName = x.Key.ProductName,
                            QuantityUnit = x.Key.QuantityUnitName,
                        });


                    //// compose order
                    //message.Sorter.Compose("completedOn", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery
                    //            .OrderBy(x => x.Year)
                    //            .ThenBy(x => x.Month)
                    //            .ThenBy(x => x.Day)
                    //        : groupedQuery
                    //            .OrderByDescending(x => x.Year)
                    //            .ThenByDescending(x => x.Month)
                    //            .ThenByDescending(x => x.Day);
                    //});

                    //message.Sorter.Compose("branchName", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.BranchName)
                    //        : groupedQuery.OrderByDescending(x => x.BranchName);
                    //});

                    //message.Sorter.Compose("supplierName", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.SupplierName)
                    //        : groupedQuery.OrderByDescending(x => x.SupplierName);
                    //});

                    //message.Sorter.Compose("categoryName", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.CategoryName)
                    //        : groupedQuery.OrderByDescending(x => x.CategoryName);
                    //});

                    //message.Sorter.Compose("productName", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.ProductName)
                    //        : groupedQuery.OrderByDescending(x => x.ProductName);
                    //});

                    //message.Sorter.Compose("quantityUnit", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.QuantityUnit)
                    //        : groupedQuery.OrderByDescending(x => x.QuantityUnit);
                    //});

                    //message.Sorter.Compose("quantityValue", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.QuantityValue)
                    //        : groupedQuery.OrderByDescending(x => x.QuantityValue);
                    //});

                    //message.Sorter.Compose("unitPriceAmount", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.UnitPriceAmount)
                    //        : groupedQuery.OrderByDescending(x => x.UnitPriceAmount);
                    //});

                    //message.Sorter.Compose("discountAmount", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.DiscountAmount)
                    //        : groupedQuery.OrderByDescending(x => x.DiscountAmount);
                    //});

                    //message.Sorter.Compose("extendedPriceAmount", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.ExtendedPriceAmount)
                    //        : groupedQuery.OrderByDescending(x => x.ExtendedPriceAmount);
                    //});

                    //message.Sorter.Compose("totalPriceAmount", direction =>
                    //{
                    //    groupedQuery = direction == SortDirection.Ascending
                    //        ? groupedQuery.OrderBy(x => x.TotalPriceAmount)
                    //        : groupedQuery.OrderByDescending(x => x.TotalPriceAmount);
                    //});

                    var countFuture = groupedQuery
                        .Select(x => x.ProductName)
                        .ToFuture();

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Count());

                    var itemsFuture = groupedQuery
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    response = new Response()
                    {
                        Count = countFuture.Count(),
                        Items = itemsFuture.ToList()
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
