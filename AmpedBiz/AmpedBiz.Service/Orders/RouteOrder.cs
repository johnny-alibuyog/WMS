using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.Orders
{
    public class RouteOrder
    {
        public class Request : Dto.OrderRoutedEvent, IRequest<Response> { }

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
                    var entity = session.Get<Order>(message.OrderId);
                    if (entity == null)
                        throw new BusinessException($"Order with id {message.OrderId} does not exists.");

                    var routedEvent = new OrderRoutedEvent(
                         routedOn: message.RoutedOn ?? DateTime.Now,
                         routedBy: session.Load<User>(message.RoutedBy.Id)
                     );

                    entity.State.Process(routedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
