using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProductInventory
	{
		public class Request : IRequest<Response>
		{
			public string Key { get; set; }

			internal Guid ProductId => Guid.TryParse(this.Key, out Guid id) ? id : Guid.Empty;

			public string Barcode => this.ProductId == Guid.Empty ? this.Key : null;
		}

		public class Response : Dto.ProductInventory { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = default(Response);

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.Query<Inventory>();

					var productId = (message.ProductId != Guid.Empty)
						? message.ProductId
						: session.Query<ProductUnitOfMeasure>()
							.Where(x => x.Barcode == message.Barcode)
							.Select(x => x.Product.Id)
							.FirstOrDefault();

					var inventory = query
						.Where(x => x.Product.Id == productId)
						.Fetch(x => x.Product)
						.ThenFetchMany(x => x.UnitOfMeasures)
						.ThenFetchMany(x => x.Prices)
						.ToFutureValue().Value;

					if (inventory == null)
					{
						return null;
					}

					response = new Response()
					{
						Id = inventory.Product.Id,
						InventoryId = inventory.Id,
						Code = inventory.Product.Code,
						Name = inventory.Product.Name,
						UnitOfMeasures = inventory.Product.UnitOfMeasures
							.Select(x => new Dto.ProductInventoryUnitOfMeasure()
							{
								Barcode = x.Barcode,
								IsDefault = x.IsDefault,
								IsStandard = x.IsStandard,
								UnitOfMeasure = x.UnitOfMeasure
										.MapTo(default(Dto.UnitOfMeasure)),
								Available = inventory
										.Convert(o => o.Available)
										.To(x.UnitOfMeasure)
										.MapTo(default(Dto.Measure)),
								TargetLevel = inventory
										.Convert(o => o.TargetLevel)
										.To(x.UnitOfMeasure)
										.MapTo(default(Dto.Measure)),
								BadStock = inventory
										.Convert(o => o.BadStock)
										.To(x.UnitOfMeasure)
										.MapTo(default(Dto.Measure)),
								Standard = x.Product
										.StandardEquivalentMeasureOf(x.UnitOfMeasure)
										.MapTo(default(Dto.Measure)),
								Prices = x.Prices
										.Select(o => new Dto.ProductInventoryUnitOfMeasurePrice()
										{
											Pricing = new Lookup<string>()
											{
												Id = o.Pricing.Id,
												Name = o.Pricing.Name
											},
											PriceAmount = o.Price.Amount
										})
										.ToList()
							})
							.ToList()
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
