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

namespace AmpedBiz.Service.Products
{
	public class CreateProduct
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
					var exists = session.Query<Product>().Any(x => x.Id == message.Id);
					exists.Assert($"Product with id {message.Id} already exists.");

                    var suppliers = session.QueryOver<Supplier>().Cacheable().List();
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
					var product = new Product(message.Id);

					product.Accept(new ProductUpdateVisitor()
					{
						Code = message.Code,
						Name = message.Name,
						Description = message.Description,
						Category = (!message.Category?.Id.IsNullOrDefault() ?? false)
							? session.Load<ProductCategory>(message.Category.Id) : null,
						Image = message.Image,
						Discontinued = message.Discontinued,
                        Suppliers = message.Suppliers
                            .Where(x => x.Assigned)
                            .Select(x => session.Load<Supplier>(x.Id))
                            .ToList(),
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
					var inventory = new Inventory(branch, product);

					inventory.Accept(new InventoryUpdateVisitor()
					{
						Branch = branch,
						Product = product,
						InitialLevel = new Measure(message.Inventory.InitialLevelValue ?? 0M, @default.UnitOfMeasure),
						TargetLevel = new Measure(message.Inventory.TargetLevelValue ?? 0M, @default.UnitOfMeasure),
						ReorderLevel = new Measure(message.Inventory.ReorderLevelValue ?? 0M, @default.UnitOfMeasure),
						MinimumReorderQuantity = new Measure(message.Inventory.MinimumReorderQuantityValue ?? 0M, @default.UnitOfMeasure),
					});

					product.EnsureValidity();

					inventory.EnsureValidity();

					session.Save(product);

                    session.Save(inventory);

					transaction.Commit();

					product.MapTo(response);

                    response.Suppliers = suppliers
                        .Select(x => new Dto.Supplier()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Assigned = product.Suppliers
                                .Select(o => o.Id)
                                .Contains(x.Id)
                        })
                        .ToList();

                    inventory.MapTo(response.Inventory);

                    SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
