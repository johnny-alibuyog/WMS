using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProductInventoryList
	{
		public class Request : IRequest<Response>
		{
			public Guid[] ProductIds { get; set; }

			public Guid SupplierId { get; set; }

			public string SearchTerm { get; set; }
		}

		public class Response : List<Dto.ProductInventory>
		{
			public Response() { }

			public Response(List<Dto.ProductInventory> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.Query<Inventory>();

					if (!message.ProductIds.IsNullOrDefault())
					{
						query = query.Where(x => message.ProductIds.Contains(x.Product.Id));
					}

					if (!message.SearchTerm.IsNullOrWhiteSpace())
					{
						query = query.Where(x =>
							x.Product.Name.StartsWith(message.SearchTerm) ||
							x.Product.UnitOfMeasures.Any(y => y.Barcode.StartsWith(message.SearchTerm))
						);
					}

					var inventories = query
						.Fetch(x => x.Product)
						.ThenFetchMany(x => x.UnitOfMeasures)
						.ThenFetchMany(x => x.Prices)
						.ToList();

					var dtos = inventories
						.Select(productInventory => new Dto.ProductInventory()
						{
							Id = productInventory.Product.Id,
							Code = productInventory.Product.Code,
							Name = productInventory.Product.Name,
							UnitOfMeasures = productInventory.Product.UnitOfMeasures
								.Select(x => new Dto.ProductInventoryUnitOfMeasure()
								{
									Barcode = x.Barcode,
									IsDefault = x.IsDefault,
									IsStandard = x.IsStandard,
									UnitOfMeasure = x.UnitOfMeasure
										.MapTo(default(Dto.UnitOfMeasure)),
                                    OnHand = productInventory
                                        .Convert(o => o.OnHand)
                                        .To(x.UnitOfMeasure)
                                        .MapTo(default(Dto.Measure)),
                                    Available = productInventory
										.Convert(o => o.Available)
										.To(x.UnitOfMeasure)
										.MapTo(default(Dto.Measure)),
									TargetLevel = productInventory
										.Convert(o => o.TargetLevel)
										.To(x.UnitOfMeasure)
										.MapTo(default(Dto.Measure)),
									BadStock = productInventory
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
						})
						.ToList();

					response = new Response(dtos);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
