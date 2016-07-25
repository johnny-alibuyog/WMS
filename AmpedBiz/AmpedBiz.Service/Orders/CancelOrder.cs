using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.Orders
{
    public class CancelOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

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
                    var entity = session.Get<Order>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    var cancelledEvent = new OrderCancelledEvent(
                        cancelledBy: session.Load<User>(message.CancelledBy.Id),
                        cancelledOn: message.CancelledOn ?? DateTime.Now,
                        cancellationReason: message.CancellationReason
                    );

                    entity.State.Process(cancelledEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
