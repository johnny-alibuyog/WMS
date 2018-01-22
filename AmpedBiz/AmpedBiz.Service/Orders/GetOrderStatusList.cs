using AmpedBiz.Service.Dto;
using MediatR;
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
            public override Response Execute(Request message)
            {
                return new Response(Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>());
            }
        }
    }
}
