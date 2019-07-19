using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using ExpressMapper.Extensions;
using MediatR;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductRetailPriceDetailsPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ProductRetailPriceDetails> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                var key = message?.Filter["key"]?.ToString();

                if (key == null)
                {
                    return response;
                }

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<ProductUnitOfMeasure>()
                        .Where(x =>
                            (
                                x.Barcode == key ||
                                x.Product.Name.Contains(key) ||
                                x.Product.Description.Contains(key)
                            )
                            &&
                            x.Prices.Any(o => o.Pricing == Pricing.RetailPrice)
                        )
                        .OrderBy(x => x.Product.Name)
                        .Select(x => x.Product.Id)
                        .Distinct();

                    var idsFuture = query
                        .Skip(message.Pager.SkipCount)
                        .Take(message.Pager.Size)
                        .ToFuture();

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = Map(idsFuture.ToArray())
                    };

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }

            private IReadOnlyList<Dto.ProductRetailPriceDetails> Map(Guid[] productIds)
            {
                var result = default(List<Dto.ProductRetailPriceDetails>);

                var session = SessionFactory.RetrieveSharedSession(Context);

                var products = session.QueryOver<Product>()
                    .Where(x => x.Id.IsIn(productIds))
                    .Fetch(x => x.UnitOfMeasures).Eager
                    .Fetch(x => x.UnitOfMeasures.First().Prices).Eager
                    .Fetch(x => x.UnitOfMeasures.First().Prices.First().Price).Eager
                    .Fetch(x => x.UnitOfMeasures.First().UnitOfMeasure).Eager
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .Future();

                var inventories = session.QueryOver<Inventory>()
                    .Where(x => 
                        x.Product.Id.IsIn(productIds) &&
                        x.Branch.Id == this.Context.BranchId
                    )
                    .TransformUsing(Transformers.DistinctRootEntity)
                    .Future();

                result = products
                    .Select(x => new Dto.ProductRetailPriceDetails()
                    {
                        ProdictId = x.Id,
                        ProductName = x.Name,
                        PriceDetails = MapDetails(
                            productId: x.Id,
                            products: products,
                            inventories: inventories
                        )
                    })
                    .ToList();

                return result;
            }

            private List<Dto.ProductRetailPriceDetailsItem> MapDetails(
                Guid productId,
                IEnumerable<Product> products,
                IEnumerable<Inventory> inventories)
            {
                var product = products.First(x => x.Id == productId);

                var inventory = inventories.First(x => x.Product.Id == productId);

                var result = product.UnitOfMeasures
                    .SelectMany(x => x.Prices)
                    .Where(x => x.Pricing == Pricing.RetailPrice)
                    .Select(x => new Dto.ProductRetailPriceDetailsItem(
                        price: x.Price.Amount,
                        total: ComputeTotal(x.Price, x.ProductUnitOfMeasure.UnitOfMeasure),
                        onHand: MapConvertedOnHand(toUnit: x.ProductUnitOfMeasure.UnitOfMeasure)
                    ))
                    .ToList();

                return result;

                decimal ComputeTotal(Money price, UnitOfMeasure unit)
                {
                    var convertedMeasure = ConvertOnHand(unit);
                    return convertedMeasure.Value * price.Amount;
                }

                Measure ConvertOnHand(UnitOfMeasure toUnit)
                {
                    var convertedMeasure = product.Convert(inventory.OnHand, toUnit);
                    convertedMeasure.Value = decimal.Truncate(convertedMeasure.Value); /* do not include deimcal places */
                    return convertedMeasure;
                }

                Dto.Measure MapConvertedOnHand(UnitOfMeasure toUnit) => ConvertOnHand(toUnit).Map(default(Dto.Measure));
            }
        }
    }
}