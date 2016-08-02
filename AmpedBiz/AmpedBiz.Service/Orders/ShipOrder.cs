using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Arguments.Orders;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.Orders
{
    public class ShipOrder
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
                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    var shippedArguments = new OrderShippedArguments()
                    { 
                        ShippedOn = message.ShippedOn ?? DateTime.Now,
                        ShippedBy = session.Load<User>(message.ShippedBy.Id)
                    };

                    entity.State.Process(shippedArguments);

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
