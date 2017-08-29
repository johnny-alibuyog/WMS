using AmpedBiz.Data.Context;
using AmpedBiz.Service.Dto;
using MediatR;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderStatusList
    {
        public class Request : IRequest<Response> { }

        public class Response : List<Dto.OrderStatus>
        {
            public Response() { }

            public Response(IEnumerable<Dto.OrderStatus> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                return new Response(Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>());
            }
        }
    }
}
