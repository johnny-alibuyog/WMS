using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Envents.PurchaseOrders;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CreateNewPurchaseOder
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
                    var entity = new PurchaseOrder();
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
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
                            .Select(x => new PurchaseOrderItem().State.New(
                                product: session.Load<Product>(x.Product.Id),
                                unitPrice: new Money(x.UnitPriceAmount, currency),
                                quantity: x.QuantityValue
                            ))
                    );

                    entity.State.New(newlyCreatedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}