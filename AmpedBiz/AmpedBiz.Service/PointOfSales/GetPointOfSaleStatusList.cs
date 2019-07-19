using AmpedBiz.Service.Dto;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PointOfSales
{
	public class GetPointOfSaleStatusList
	{
		public class Request : IRequest<Response> { }

		public class Response : List<PointOfSaleStatus>
		{
			public Response() { }

			public Response(IEnumerable<PointOfSaleStatus> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				return new Response(Enum.GetValues(typeof(PointOfSaleStatus)).Cast<PointOfSaleStatus>());
			}
		}
	}
}
