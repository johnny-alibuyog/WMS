using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Inventories
{
	public class GetInventoryAdjustmentReasonList
	{
		public class Request : IRequest<Response>
		{
			public InventoryAdjustmentType? Type { get; set; }
		}

		public class Response : List<Dto.InventoryAdjustmentReason>
		{
			public Response() { }

			public Response(IList<Dto.InventoryAdjustmentReason> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{

					var entities = session.Query<InventoryAdjustmentReason>()
						.OrderBy(x => x.Name)
						.Cacheable()
						.ToList();

					if (message.Type != null)
						entities = entities.Where(x => x.Type == message.Type.Value).ToList();

					var dtos = entities.MapTo(default(List<Dto.InventoryAdjustmentReason>));

					response = new Response(dtos);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}