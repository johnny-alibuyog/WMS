using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using Newtonsoft.Json.Linq;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetNeedsReorderingPage
    {
        public class Request : PageRequest, IRequest<Response>
        {
            public bool? UseDefaultUnitOfMeasure { get; set; }
        }

        public class Response : PageResponse<Dto.NeedsReorderingPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Inventory>()
                        .Where(x =>
                            (x.ReorderLevel != null && x.ReorderLevel.Value > 0M) &&
                            (
                                (x.CurrentLevel == null) ||
                                (x.CurrentLevel.Value <= x.ReorderLevel.Value)
                            )
                        );

                    // compose filters
                    message.Filter.Compose<Guid>("supplierId", value =>
                    {
                        query = query.Where(x => x.Product.Supplier.Id == value);
                    });

                    var purchaseAllBellowTarget = message.Filter.ContainsKey("purchaseAllBelowTarget")
                        ? (bool)message.Filter["purchaseAllBelowTarget"] : true;

                    var selectedProductIds = message.Filter.ContainsKey("selectedProductIds")
                        ? ((JArray)message.Filter["selectedProductIds"]).Select(x => Guid.Parse(((string)x)))
                        : new Guid[] { };
                    
                    if (purchaseAllBellowTarget != true && selectedProductIds.Any())
                    {
                        query = query.Where(x => selectedProductIds.Contains(x.Product.Id));
                    }

                    // compose sort
                    message.Sorter.Compose("code", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Product.Code)
                            : query.OrderByDescending(x => x.Product.Code);
                    });

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

                    message.Sorter.Compose("reorderLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReorderLevel.Value)
                            : query.OrderByDescending(x => x.ReorderLevel.Value);
                    });

                    message.Sorter.Compose("available", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.Available.Value)
                            : query.OrderByDescending(x => x.Available.Value);
                    });

                    message.Sorter.Compose("currentLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.CurrentLevel.Value)
                            : query.OrderByDescending(x => x.CurrentLevel.Value);
                    });

                    message.Sorter.Compose("targetLevel", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.TargetLevel.Value)
                            : query.OrderByDescending(x => x.TargetLevel.Value);
                    });

                    message.Sorter.Compose("belowTarget", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.BelowTargetLevel.Value)
                            : query.OrderByDescending(x => x.BelowTargetLevel.Value);
                    });

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsRawFuture = query
                        .Select(x => new
                        {
                            ProductId = x.Product.Id,
                            ProductName = x.Product.Name,
                            ProductCode = x.Product.Code,
                            SupplierName = x.Product.Supplier.Name,
                            CategoryName = x.Product.Category.Name,
                            ReorderLevel = x.ReorderLevel,
                            Available = x.Available,
                            CurrentLevel = x.CurrentLevel,
                            TargetLevel = x.TargetLevel,
                            BelowTargetLevel = x.BelowTargetLevel,
                            MinimumReorderQuantity = x.MinimumReorderQuantity
                        })
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var productIds = itemsRawFuture.Select(o => o.ProductId).ToArray();

                    var products = session.QueryOver<Product>()
                        .WhereRestrictionOn(x => x.Id).IsIn(productIds)
                        .Fetch(x => x.UnitOfMeasures).Eager
                        .Fetch(x => x.UnitOfMeasures.First().UnitOfMeasure).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices.First().Price).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .List().ToDictionary(x => x.Id);


                    var getConvertedValue = new Func<Guid, Measure, decimal?>((productId, measure) =>
                    {
                        return (message.UseDefaultUnitOfMeasure == null || message.UseDefaultUnitOfMeasure.Value)
                            ? products[productId].ConvertToDefaultValue(measure)
                            : products[productId].ConvertToStandardValue(measure);
                    });

                    var getUnitOfMeasure = new Func<Guid, string>((productId) =>
                    {
                        return (message.UseDefaultUnitOfMeasure == null || message.UseDefaultUnitOfMeasure.Value)
                            ? products[productId].UnitOfMeasures.Default(x => x.UnitOfMeasure.Name)
                            : products[productId].UnitOfMeasures.Standard(x => x.UnitOfMeasure.Name);
                    });

                    var items = itemsRawFuture
                        .Select(x => new Dto.NeedsReorderingPageItem()
                        {
                            Id = x.ProductId.ToString(),
                            ProductName = x.ProductName,
                            ProductCode = x.ProductCode,
                            SupplierName = x.SupplierName,
                            CategoryName = x.CategoryName,
                            UnitOfMeasureName = getUnitOfMeasure(x.ProductId),
                            ReorderLevelValue = getConvertedValue(x.ProductId, x.ReorderLevel),
                            AvailableValue = getConvertedValue(x.ProductId, x.Available),
                            CurrentLevelValue = getConvertedValue(x.ProductId, x.CurrentLevel),
                            TargetLevelValue = getConvertedValue(x.ProductId, x.TargetLevel),
                            BelowTargetValue = getConvertedValue(x.ProductId, x.BelowTargetLevel),
                            MinimumReorderQuantity = getConvertedValue(x.ProductId, x.MinimumReorderQuantity),
                        });

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = items.ToList()
                    };

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
