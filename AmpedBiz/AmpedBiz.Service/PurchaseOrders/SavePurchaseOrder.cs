using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.PurchaseOrders;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
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
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
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
                            .Fetch(x => x.Items.First().Product.Inventory).Eager
                            .Fetch(x => x.Payments).Eager
                            .Fetch(x => x.Payments.First().PaidBy).Eager
                            .Fetch(x => x.Payments.First().PaymentType).Eager
                            .Fetch(x => x.Receipts).Eager
                            .Fetch(x => x.Receipts.First().Product).Eager
                            .Fetch(x => x.Receipts.First().Product.Inventory).Eager
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
                        .Fetch(x => x.Inventory)
                        .ToList();

                    Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<Guid, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    entity.Accept(new PurchaseOrderUpdateVisitor()
                    { 
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
                            quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id))
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
                            quantity: new Measure(
                                value: x.QuantityValue,
                                unit: products
                                    .Where(o => o.Id == x.Product.Id)
                                    .Select(o => o.Inventory.UnitOfMeasure)
                                    .FirstOrDefault()
                            )
                        ))
                    });
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}