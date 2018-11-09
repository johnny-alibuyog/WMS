using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Core.PointOfSales.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using AmpedBiz.Data.Helpers;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PointOfSales
{
	public class SavePointOfSale
	{
		public class Request : Dto.PointOfSale, IRequest<Response>
		{
			public virtual Guid UserId { get; set; }
		}

		public class Response : Dto.PointOfSale { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var settings = new SettingsFacade(session);

					var productIds = message.Items.Select(x => x.Product.Id);

					var products = session.Query<Product>()
						.Where(x => productIds.Contains(x.Id))
						.Fetch(x => x.Inventories)
						.FetchMany(x => x.UnitOfMeasures)
						.ThenFetchMany(x => x.Prices)
						.ToList();

					var GetProduct = new Func<Guid, Product>(id => products.First(x => x.Id == id));

					var entity = new PointOfSale();

					entity.Accept(new PointOfSaleUpdateVisitor()
					{
						Branch = message.Branch?.Id.EvalOrDefault(value => session.Load<Branch>(value)),
						Customer = message.Customer?.Id.EvalOrDefault(value => session.Load<Customer>(value)),
						Pricing = message.Pricing?.Id.EvalOrDefault(value => session.Load<Pricing>(value)),
						PaymentType = message.PaymentType?.Id.EvalOrDefault(value => session.Load<PaymentType>(value)),
						TendredBy = message.TendredBy?.Id.EvalOrDefault(value => session.Load<User>(value)),
						Items = message.Items
							.Select(x => new PointOfSaleItem(
								id: x.Id,
								discountRate: x.DiscountRate,
								product: GetProduct(x.Product.Id),
								unitPrice: new Money(x.UnitPriceAmount, settings.DefaultCurrency),
								quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
								standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id))
							))
							.ToList(),
						Payments = message.Payments
							.Select(x => new PointOfSalePayment(
								id: x.Id,
								paymentType: session.Load<PaymentType>(x.PaymentType.Id),
								payment: new Money(x.PaymentAmount, settings.DefaultCurrency),
								balance: new Money(x.BalanceAmount, settings.DefaultCurrency)
							))
							.ToList(),
					});

					entity.EnsureValidity();

					session.Save(entity);

					transaction.Commit();

					entity.MapTo(response);

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
