using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.Orders
{
    public class CompleteOrder
    {
        public class Request : Dto.OrderCompletedEvent, IRequest<Response> { }

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

                    var completedEvent = new OrderCompletedEvent(
                        completedBy: session.Load<User>(message.CompletedBy.Id),
                        completedOn: message.CompletedOn ?? DateTime.Now
                    );

                    entity.State.Process(completedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    //todo: not working when mapped to invoice
                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}