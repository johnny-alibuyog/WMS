using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Inventories.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Products.Services;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;
using static AmpedBiz.Common.Extentions.TypeExtentions;

namespace AmpedBiz.Service.Products
{
	public class UpdateProduct
    {
        public class Request : Dto.Product, IRequest<Response> { }

        public class Response : Dto.Product { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var product = session.Query<Product>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Inventories)
                        .FetchMany(x => x.UnitOfMeasures)
                        .ThenFetchMany(x => x.Prices)
                        .ToFutureValue().Value;

                    product.EnsureExistence($"Product with id {message.Id} does not exists.");

                    product.Accept(new ProductUpdateVisitor()
                    {
                        Code = message.Code,
                        Name = message.Name,
                        Description = message.Description,
                        Supplier = (!message.Supplier?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Supplier>(message.Supplier.Id) : null,
                        Category = (!message.Category?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<ProductCategory>(message.Category.Id) : null,
                        Image = message.Image,
                        Discontinued = message.Discontinued,
                        UnitOfMeasures = message.UnitOfMeasures
                            .Select(x => new ProductUnitOfMeasure(
                                id: x.Id,
                                size: x.Size,
                                barcode: x.Barcode,
                                isDefault: x.IsDefault ?? false,
                                isStandard: x.IsStandard ?? false,
                                unitOfMeasure: session.Load<UnitOfMeasure>(x.UnitOfMeasure.Id),
                                standardEquivalentValue: x.StandardEquivalentValue ?? 0M,
                                prices: x.Prices
                                    .Select(o => new ProductUnitOfMeasurePrice(
                                        id: o.Id,
                                        pricing: session.Load<Pricing>(o.Pricing.Id),
                                        price: new Money(amount: o.PriceAmount ?? 0M, currency: currency)
                                    ))
                                    .ToList()
                            ))
                            .ToList()
                    });

                    var @default = product.UnitOfMeasures.FirstOrDefault(o => o.IsDefault);
                    var branch = session.Load<Branch>(this.Context.BranchId);
                    var inventory = session.Query<Inventory>().FirstOrDefault(x => x.Product == product && x.Branch == branch);
                    if (inventory == null)
                    {
                        inventory = new Inventory(branch, product);
                        session.Save(inventory);
                    }

                    inventory.Accept(new InventoryUpdateVisitor()
                    {
                        Branch = branch,
                        Product = product,
                        InitialLevel = new Measure(message.Inventory.InitialLevelValue ?? 0M, @default.UnitOfMeasure),
                        TargetLevel = new Measure(message.Inventory.TargetLevelValue ?? 0M, @default.UnitOfMeasure),
                        ReorderLevel = new Measure(message.Inventory.ReorderLevelValue ?? 0M, @default.UnitOfMeasure),
                        MinimumReorderQuantity = new Measure(message.Inventory.MinimumReorderQuantityValue ?? 0M, @default.UnitOfMeasure)
                    });

                    product.EnsureValidity();
                    inventory.EnsureValidity();

                    transaction.Commit();

                    product.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}