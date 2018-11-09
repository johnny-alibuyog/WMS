using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProductUnitOfMeasure
	{
		public class Request : IRequest<Response>
		{
			public string Key { get; set; }

			internal Guid Id => Guid.TryParse(this.Key, out Guid id) ? id : Guid.Empty;

			public string Barcode => this.Id == Guid.Empty ? this.Key : null;
		}

		public class Response : Dto.ProductUnitOfMeasure { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = default(Response);

				if (string.IsNullOrWhiteSpace(message.Key))
				{
					throw new ArgumentNullException($"{nameof(Request.Key)} should contain value.");
				}
					
				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.QueryOver<ProductUnitOfMeasure>()
						.Fetch(x => x.Product).Eager
						.Fetch(x => x.Product.UnitOfMeasures).Eager
						.Fetch(x => x.Product.UnitOfMeasures.First().Prices).Eager;

					if (message.Id != Guid.Empty)
					{
						query = query.Where(x => x.Id == message.Id);
					}

					if (!string.IsNullOrWhiteSpace(message.Barcode))
					{
						query = query.Where(x => x.Barcode == message.Barcode);
					}

					var entity = query.SingleOrDefault();

					entity.EnsureExistence();

					response = new Response()
					{
						Id = entity.Id,
						ProductId = entity.Product.Id,
						Size = entity.Size,
						Barcode = entity.Barcode,
						IsDefault = entity.IsDefault,
						IsStandard = entity.IsStandard,
						UnitOfMeasure = new Dto.UnitOfMeasure(
							id: entity.UnitOfMeasure.Id,
							name: entity.UnitOfMeasure.Name
						),
						StandardEquivalentValue = entity.StandardEquivalentValue,
						Prices = entity.Prices
							.Select(x => new Dto.ProductUnitOfMeasurePrice()
							{
								Id = x.Id,
								Pricing = new Lookup<string>(
									id: x.Pricing?.Id,
									name: x.Pricing?.Name
								),
								PriceAmount = x.Price?.Amount
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
