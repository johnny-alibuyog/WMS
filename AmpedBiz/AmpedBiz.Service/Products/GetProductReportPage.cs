using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductReportPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductReportPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Inventory>();

                    //var pricingId = message.Filter.GetValueOrDefault("pricingId") as string ?? Pricing.BasePrice.Id;

                    var useDefault = (message.Filter.GetValueOrDefault("measureType") as string == "default" /* values: "default" or "standard" */); 

                    // compose filter
                    message.Filter.Compose<Guid>("productId", value =>
                    {
                        query = query.Where(x => x.Product.Id == value);
                    });

                    message.Filter.Compose<string>("categoryId", value =>
                    {
                        query = query.Where(x => x.Product.Category.Id == value);
                    });

                    message.Filter.Compose<Guid>("supplierId", value =>
                    {
                        query = query.Where(x => x.Product.Supplier.Id == value);
                    });

                    // compose order
                    message.Sorter.Compose("productName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Product.Name)
                            : query.OrderByDescending(x => x.Product.Name);
                    });

                    message.Sorter.Compose("categoryName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Product.Category.Name)
                            : query.OrderByDescending(x => x.Product.Category.Name);
                    });

                    message.Sorter.Compose("supplierName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Product.Supplier.Name)
                            : query.OrderByDescending(x => x.Product.Supplier.Name);
                    });

                    message.Sorter.Compose("onHandValue", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.OnHand.Value)
                            : query.OrderByDescending(x => x.OnHand.Value);
                    });

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new
                        {
                            Id = x.Product.Id,
                            ProductCode = x.Product.Code,
                            ProductName = x.Product.Name,
                            CategoryName = x.Product.Category.Name,
                            SupplierName = x.Product.Supplier.Name,
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var productIds = itemsFuture.Select(x => x.Id).ToArray();

                    var products = session.QueryOver<Product>()
                        .WhereRestrictionOn(x => x.Id).IsIn(productIds)
                        .Fetch(x => x.UnitOfMeasures).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices.First().Price).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices.First().Pricing).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .Future();

                    var inventories = session.Query<Inventory>()
                        .Where(x => productIds.Contains(x.Product.Id))
                        .Fetch(x => x.OnHand)
                        .ToFuture();

                    var inventoryLookup = inventories.ToList()
                        .ToDictionary(x => x.Product.Id);

                    var lookup = products.ToList()
                        .ToDictionary(x => x.Id, product => new
                        {
                            OnHand = useDefault
                                ? product.ConvertToDefault(inventoryLookup[product.Id].OnHand)
                                : product.ConvertToStandard(inventoryLookup[product.Id].OnHand),
                            UnitOfMeasure = useDefault
                                ? product.UnitOfMeasures.Default(o => o.UnitOfMeasure)
                                : product.UnitOfMeasures.Standard(o => o.UnitOfMeasure),
                            BasePrice = useDefault 
                                ? product.UnitOfMeasures.Default(o => o.Prices.Base())
                                : product.UnitOfMeasures.Standard(o => o.Prices.Base()),
                            WholesalePrice = useDefault
                                ? product.UnitOfMeasures.Default(o => o.Prices.Wholesale())
                                : product.UnitOfMeasures.Standard(o => o.Prices.Wholesale()),
                            RetailPrice = useDefault
                                ? product.UnitOfMeasures.Default(o => o.Prices.Retail())
                                : product.UnitOfMeasures.Standard(o => o.Prices.Retail()),
                        });

                    var items = itemsFuture
                        .Select(x => new Dto.ProductReportPageItem()
                        {
                            Id = x.Id,
                            ProductCode = x.ProductCode,
                            ProductName = x.ProductName,
                            CategoryName = x.CategoryName,
                            SupplierName = x.SupplierName,
                            OnHandUnit = lookup[x.Id].OnHand?.Unit?.Id,
                            OnHandValue = lookup[x.Id].OnHand?.Value,
                            BasePriceAmount = lookup[x.Id].BasePrice?.Amount,
                            WholesalePriceAmount = lookup[x.Id].WholesalePrice?.Amount,
                            RetailPriceAmount = lookup[x.Id].RetailPrice?.Amount,
                        });

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = items.ToList()
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
