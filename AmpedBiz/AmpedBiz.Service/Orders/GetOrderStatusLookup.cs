using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using MediatR;
using System.Collections.Generic;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderStatusLookup
    {
        public class Request : IRequest<Response> { }

        public class Response : List<Lookup<Dto.OrderStatus>>
        {
            public Response() { }

            public Response(List<Lookup<Dto.OrderStatus>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                return new Response(EnumExtention.ToLookup<Dto.OrderStatus>());
            }
        }
    }
}
