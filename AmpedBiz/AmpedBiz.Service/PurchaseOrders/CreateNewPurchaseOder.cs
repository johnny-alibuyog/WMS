using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CreateNewPurchaseOder
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
                    var entity = new PurchaseOrder();

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant

                    entity.State.New(
                        createdBy: !message.UserId.IsNullOrDefault() 
                            ? session.Load<User>(message.UserId) : null,
                        createdOn: DateTime.Now,
                        expectedOn: message.ExpectedOn,
                        paymentType: !message.PaymentTypeId.IsNullOrEmpty() 
                            ? session.Load<PaymentType>(message.PaymentTypeId) : null,
                        shipper: null,
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        tax: new Money(message.TaxAmount, currency),
                        supplier: session.Load<Supplier>(message.SupplierId),
                        purchaseOrderItems: message.Items
                            .Select(x => new PurchaseOrderItem().State.New(
                                product: session.Load<Product>(x.Product.Id),
                                unitPrice: new Money(x.UnitPriceAmount, currency),
                                quantity: x.QuantityValue
                            ))
                    );

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}