using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.PurchaseOrders;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class PayPurchaseOrder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            private void Hydrate(Response response)
            {
                var handler = new GetPurchaseOrder.Handler(this._sessionFactory);
                var hydrated = handler.Handle(new GetPurchaseOrder.Request(response.Id));

                hydrated.MapTo(response);
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); //TODO: this should be taken from tenant

                    entity.State.Process(new PurchaseOrderUpdatePaymentsVisitor()
                    {
                        Payments = message.Payments.Select(x => new PurchaseOrderPayment(
                            paidOn: x.PaidOn ?? DateTime.Now,
                            paidBy: session.Load<User>(x.PaidBy.Id),
                            paymentType: session.Load<PaymentType>(x.PaymentType.Id),
                            payment: new Money(x.PaymentAmount, currency)
                        ))
                    });

                    session.Save(entity);
                    transaction.Commit();

                    response.Id = entity.Id;
                    //entity.MapTo(response);
                }

                Hydrate(response);

                return response;
            }
        }
    }
}