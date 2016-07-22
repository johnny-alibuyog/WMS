﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Envents.PurchaseOrders;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;


namespace AmpedBiz.Service.PurchaseOrders
{
    public class UpdateNewPurchaseOder
    {
        public class Request : Dto.PurchaseOrderNewlyCreatedEvent, IRequest<Response> { }

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
                    var entity = session.Get<PurchaseOrder>(message.PurchaseOrderId);

                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.PurchaseOrderId} does not exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant

                    var productIds = message.Items.Select(x => x.Product.Id);

                    var products = session.Query<Product>()
                        .Where(x => productIds.Contains(x.Id))
                        .Fetch(x => x.Inventory)
                        .ToList();

                    Func<string, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<string, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    var newlyCreatedEvent = new PurchaseOrderNewlyCreatedEvent(
                        createdBy: (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        createdOn: message?.CreatedOn ?? DateTime.Now,
                        expectedOn: message?.ExpectedOn,
                        paymentType: (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        supplier: (!message?.Supplier?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Supplier>(message.Supplier.Id) : null,
                        shipper: null,
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        tax: new Money(message.TaxAmount, currency),
                        purchaseOrderItems: message.Items
                            .Select(x => new PurchaseOrderItem(
                                product: GetProduct(x.Product.Id),
                                unitCost: new Money(x.UnitCostAmount, currency),
                                quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id))
                            ))
                    );

                    entity.State.Process(newlyCreatedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
