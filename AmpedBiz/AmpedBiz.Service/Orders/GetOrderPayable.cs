using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderPayable
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : Dto.OrderPayable { }

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
                    entity.EnsureExistence($"Order with id {message.Id} does not exists.");
                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
