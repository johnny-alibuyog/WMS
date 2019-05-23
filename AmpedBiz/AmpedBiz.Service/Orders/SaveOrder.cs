using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Orders;
using AmpedBiz.Core.Orders.Services;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
	public class SaveOrder
	{
		public class Request : Dto.Order, IRequest<Response>
		{
			public virtual Guid UserId { get; set; }
		}

		public class Response : Dto.Order { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					//session.EnableBranchFilter(this.Context.BranchId);

					var entity = (Order)null;

					if (message.Id != Guid.Empty)
					{
						entity = session.QueryOver<Order>()
							.Where(x => x.Id == message.Id)
							.Fetch(x => x.Branch).Eager
							.Fetch(x => x.Customer).Eager
							.Fetch(x => x.Pricing).Eager
							.Fetch(x => x.PaymentType).Eager
							.Fetch(x => x.Shipper).Eager
							.Fetch(x => x.Tax).Eager
							.Fetch(x => x.ShippingFee).Eager
							.Fetch(x => x.Discount).Eager
							.Fetch(x => x.SubTotal).Eager
							.Fetch(x => x.Total).Eager
							.Fetch(x => x.CreatedBy).Eager
							.Fetch(x => x.OrderedBy).Eager
							.Fetch(x => x.RoutedBy).Eager
							.Fetch(x => x.StagedBy).Eager
							.Fetch(x => x.InvoicedBy).Eager
							.Fetch(x => x.PaymentBy).Eager
							.Fetch(x => x.RoutedBy).Eager
							.Fetch(x => x.CompletedBy).Eager
							.Fetch(x => x.CancelledBy).Eager
							.Fetch(x => x.Items).Eager
							.Fetch(x => x.Items.First().Product).Eager
							.Fetch(x => x.Items.First().Product.Inventories).Eager
							.Fetch(x => x.Payments).Eager
							.Fetch(x => x.Payments.First().PaymentBy).Eager
							.Fetch(x => x.Payments.First().PaymentType).Eager
							.Fetch(x => x.Returns).Eager
							.Fetch(x => x.Returns.First().Reason).Eager
							.Fetch(x => x.Returns.First().ReturnedBy).Eager
							.Fetch(x => x.Returns.First().Product).Eager
							.Fetch(x => x.Returns.First().Product.Inventories).Eager
							.SingleOrDefault();

						entity.EnsureExistence($"Order with id {message.Id} does not exists.");
					}
					else
					{
						entity = new Order();
					}

					var currency = session.Load<Currency>(Currency.PHP.Id);

					var productIds = message.Items.Select(x => x.Product.Id);

					var products = session.Query<Product>()
						.Where(x => productIds.Contains(x.Id))
						.Fetch(x => x.Inventories)
						.FetchMany(x => x.UnitOfMeasures)
						.ThenFetchMany(x => x.Prices)
						.ToList();

					Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

					entity.Accept(new OrderUpdateVisitor()
					{
						OrderNumber = message.OrderNumber,
						CreatedBy = message.CreatedBy?.Id.EvalOrDefault(value => session.Load<User>(value)),
						CreatedOn = message.CreatedOn.GetValueOrDefault(DateTime.Now),
						OrderedBy = message.OrderedBy?.Id.EvalOrDefault(value => session.Load<User>(value)),
						OrderedOn = message.OrderedOn.GetValueOrDefault(DateTime.Now),
						Branch = message.Branch?.Id.EvalOrDefault(value => session.Load<Branch>(value)),
						Customer = message.Customer?.Id.EvalOrDefault(value => session.Load<Customer>(value)),
						Shipper = message.Shipper?.Id.EvalOrDefault(value => session.Load<Shipper>(value)),
						ShippingAddress = message.ShippingAddress.EvalOrDefault(value => value.MapTo<Dto.Address, Address>()),
						Pricing = message.Pricing?.Id.EvalOrDefault(value => session.Load<Pricing>(value)),
						PaymentType = message.PaymentType?.Id.EvalOrDefault(value => session.Load<PaymentType>(value)),
						TaxRate = message.TaxRate,
						Tax = new Money(message.TaxAmount, currency),
						ShippingFee = new Money(message.ShippingFeeAmount, currency),
						Items = message.Items
							.Select((x, i) => new OrderItem(
								id: x.Id,
                                sequence: i,
								discountRate: x.DiscountRate,
								product: GetProduct(x.Product.Id),
								unitPrice: new Money(x.UnitPriceAmount, currency),
								quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
								standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id))
							))
							.ToList(),
						Payments = message.Payments
							.Select((x, i) => new OrderPayment(
								id: x.Id,
                                sequence: i,
								paymentOn: x.PaymentOn.GetValueOrDefault(DateTime.Now),
								paymentBy: session.Load<User>(x.PaymentBy.Id),
								paymentType: session.Load<PaymentType>(x.PaymentType.Id),
								payment: new Money(x.PaymentAmount, currency),
								balance: new Money(x.BalanceAmount, currency)
							))
							.ToList(),
						Returns = message.Returns
							.Select((x, i) => new OrderReturn(
								id: x.Id,
                                sequence: i,
								product: GetProduct(x.Product.Id),
								reason: x.Reason?.Id.EvalOrDefault(value => session.Load<ReturnReason>(value)),
								returnedOn: message.ReturnedOn.GetValueOrDefault(DateTime.Now),
								returnedBy: x.ReturnedBy?.Id.EvalOrDefault(value => session.Load<User>(value)),
								quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
								standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id)),
								returned: new Money(0M, currency)
							// NOTE: Since the payment is not yet fulfilled and, return money in this 
							//		 transaction is not applicable. If the customer demands a return for 
							//		 orders that has been completed/paid, it should be done in Returns module.
							))
							.ToList()
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