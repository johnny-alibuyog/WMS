using System;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using AmpedBiz.Core.Events.Orders;

namespace AmpedBiz.Service.Orders
{
    public class PayOrder
    {
        public class Request : Dto.OrderPaidEvent, IRequest<Response> { }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory)
            {
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Order>(message.OrderId);

                    if (entity == null)
                        throw new BusinessException($"Order with id {message.OrderId} does not exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var paidEvent = new OrderPaidEvent(
                        paidOn: message.PaidOn ?? DateTime.Now,
                        paidBy: session.Load<User>(message.PaidBy.Id),
                        paymentType: session.Load<PaymentType>(message.PaymentType.Id)
                        //payment: new Money(message.PaymentAmount, currency)
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
