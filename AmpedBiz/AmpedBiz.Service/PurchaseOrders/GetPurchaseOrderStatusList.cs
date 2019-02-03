using AmpedBiz.Service.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderStatusList
    {
        public class Request : IRequest<Response> { }

        public class Response : List<PurchaseOrderStatus>
        {
            public Response() { }

            public Response(IEnumerable<PurchaseOrderStatus> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                return new Response(Enum.GetValues(typeof(PurchaseOrderStatus)).Cast<PurchaseOrderStatus>());
            }
        }
    }
}
