using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class PayOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Order>(message.Id);

                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var paidEvent = new OrderPaidEvent(
                        payments: message.Payments.Select(x => new OrderPayment(
                            paidOn: x.PaidOn ?? DateTime.Now,
                            paidBy: session.Load<User>(x.PaidBy.Id),
                            paymentType: session.Load<PaymentType>(x.PaymentType.Id),
                            payment: new Money(x.PaymentAmount, currency)
                        ))
                    );

                    entity.State.Process(paidEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
