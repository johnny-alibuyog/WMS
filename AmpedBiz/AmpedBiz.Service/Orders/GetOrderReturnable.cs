using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderReturnable
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : List<Dto.OrderReturnable>
        {
            public Response() { }

            public Response(List<Dto.OrderReturnable> items): base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                if (message.Id == Guid.Empty)
                    return response;

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var orderItems = session.QueryOver<OrderItem>()
                        .Where(x => x.Order.Id == message.Id)
                        .Fetch(x => x.Product).Eager
                        .Fetch(x => x.Product.Inventory).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .List();

                    orderItems.EnsureExistence($"Order with id {message.Id} does not exists.");

                    var dtos = orderItems.MapTo((List<Dto.OrderReturnable>)null);
                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
