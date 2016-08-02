using System;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using AmpedBiz.Core.Arguments.Orders;

namespace AmpedBiz.Service.Orders
{
    public class StageOrder
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

                    var stagedArguments = new OrderStagedArguments()
                    { 
                        StagedOn = message.StagedOn ?? DateTime.Today,
                        StagedBy = session.Load<User>(message.StagedBy.Id)
                    };

                    entity.State.Process(stagedArguments);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}