using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Inventories
{
    public class GetInventoryAdjustmentTypeList
    {
        public class Request : IRequest<Response> { }

        public class Response : List<Dto.InventoryAdjustmentType>
        {
            public Response() { }

            public Response(IEnumerable<Dto.InventoryAdjustmentType> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                return new Response(Enum.GetValues(typeof(Dto.InventoryAdjustmentType)).Cast<Dto.InventoryAdjustmentType>());
            }
        }
    }
}
