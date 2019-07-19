using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using MediatR;
using System.Collections.Generic;

namespace AmpedBiz.Service.PointOfSales
{
    public class GetPointOfSaleStatusLookup
	{
        public class Request : IRequest<Response> { }

        public class Response : List<Lookup<Dto.PointOfSaleStatus>>
        {
            public Response() { }

            public Response(List<Lookup<Dto.PointOfSaleStatus>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                return new Response(EnumExtention.ToLookup<Dto.PointOfSaleStatus>());
            }
        }
    }
}
