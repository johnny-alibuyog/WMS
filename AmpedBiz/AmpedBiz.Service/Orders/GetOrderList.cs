using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }
        }

        public class Response : List<Dto.Order>
        {
            public Response() { }

            public Response(List<Dto.Order> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<Order>().ToList();
                    var dtos = entites.MapTo(default(List<Dto.Order>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}