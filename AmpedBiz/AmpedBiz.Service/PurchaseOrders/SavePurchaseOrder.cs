using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Core.PurchaseOrders.Services;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
	public class SavePurchaseOrder
	{
		public class Request : Dto.PurchaseOrder, IRequest<Response> { }

		public class Response : Dto.PurchaseOrder { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entity = (PurchaseOrder)null;

					if (message.Id != Guid.Empty)
					{
						entity = session.QueryOver<PurchaseOrder>()
							.Where(x => x.Id == message.Id)
							.Fetch(x => x.CreatedBy).Eager
							.Fetch(x => x.PaymentType).Eager
							.Fetch(x => x.Supplier).Eager
							.Fetch(x => x.Shipper).Eager
							.Fetch(x => x.ShippingFee).Eager
							.Fetch(x => x.Tax).Eager
							.Fetch(x => x.Items).Eager
							.Fetch(x => x.Items.First().Product).Eager
							.Fetch(x => x.Items.First().Product.Inventories).Eager
							.Fetch(x => x.Payments).Eager
							.Fetch(x => x.Payments.First().PaidBy).Eager
							.Fetch(x => x.Payments.First().PaymentType).Eager
							.Fetch(x => x.Receipts).Eager
							.Fetch(x => x.Receipts.First().Product).Eager
							.Fetch(x => x.Receipts.First().Product.Inventories).Eager
							.SingleOrDefault();

						entity.EnsureExistence($"Order with id {message.Id} does not exists.");
					}
					else
					{
						entity = new PurchaseOrder();
					}

					var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant

					var productIds =
						(message.Items.Select(x => x.Product.Id))
						.Union
						(message.Receipts.Select(x => x.Product.Id));

					var products = session.Query<Product>()
						.Where(x => productIds.Contains(x.Id))
						.Fetch(x => x.Inventories)
						.ToList();

					Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

					entity.Accept(new PurchaseOrderUpdateVisitor()
					{
						Branch = session.Load<Branch>(Context.BranchId),
						ReferenceNumber = message.ReferenceNumber,
						CreatedBy = (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
							? session.Load<User>(message.CreatedBy.Id) : null,
						CreatedOn = message?.CreatedOn ?? DateTime.Now,
						ExpectedOn = message?.ExpectedOn,
						PaymentType = (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
							? session.Load<PaymentType>(message.PaymentType.Id) : null,
						Supplier = (!message?.Supplier?.Id.IsNullOrDefault() ?? false)
							? session.Load<Supplier>(message.Supplier.Id) : null,
						Shipper = null,
						ShippingFee = new Money(message.ShippingFeeAmount, currency),
						Tax = new Money(message.TaxAmount, currency),
						Items = message.Items.Select(x => new PurchaseOrderItem(
							id: x.Id,
							product: GetProduct(x.Product.Id),
							unitCost: new Money(x.UnitCostAmount, currency),
							quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
							standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id))
						)),
						Payments = message.Payments.Select(x => new PurchaseOrderPayment(
							id: x.Id,
							paidOn: x.PaidOn ?? DateTime.Now,
							paidBy: session.Load<User>(x.PaidBy.Id),
							paymentType: session.Load<PaymentType>(x.PaymentType.Id),
							payment: new Money(x.PaymentAmount, currency)
						)),
						Receipts = message.Receipts.Select(x => new PurchaseOrderReceipt(
							id: x.Id,
							batchNumber: x.BatchNumber,
							receivedBy: session.Load<User>(x.ReceivedBy.Id),
							receivedOn: x.ReceivedOn ?? DateTime.Now,
							expiresOn: x.ExpiresOn,
							product: products.FirstOrDefault(o => o.Id == x.Product.Id),
							quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
							standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id))
						))
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