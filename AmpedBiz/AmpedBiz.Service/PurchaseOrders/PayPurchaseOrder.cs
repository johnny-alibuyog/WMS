using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Envents.PurchaseOrders;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class PayPurchaseOrder
    {
        public class Request : Dto.PurchaseOrderPaidEvent, IRequest<Response> { }

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

                    var currency = session.Load<Currency>(Currency.PHP.Id); //TODO: this should be taken from tenant
                    var paidEvent = new PurchaseOrderPaidEvent(
                        payments: message.Payments.Select(x => new PurchaseOrderPayment(
                            paidOn: x.PaidOn ?? DateTime.Now,
                            paidBy: session.Load<User>(x.PaidBy.Id),
                            paymentType: session.Load<PaymentType>(x.PaymentType.Id),
                            payment: new Money(x.PaymentAmount, currency)
                        )
                    ));

                    entity.State.Pay(paidEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}