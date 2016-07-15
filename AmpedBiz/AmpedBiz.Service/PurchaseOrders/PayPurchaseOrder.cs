using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class PayPurchaseOrder
    {
        public class Request : Dto.PurchaseOrderPayment, IRequest<Response> { }

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

                    entity.State.Pay(
                        paidOn: message.PaidOn ?? DateTime.Now,
                        paidBy: session.Load<User>(message.PaidBy.Id), 
                        paymentType: session.Load<PaymentType>(message.PaymentType.Id),
                        payment: new Money(message.PaymentAmount, session.Load<Currency>(Currency.PHP.Id))
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