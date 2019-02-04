using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProductLookup
	{
		public class Request : IRequest<Response>
		{
			public Guid[] Id { get; set; }

			public Guid SupplierId { get; set; }
		}

		public class Response : List<Lookup<Guid>>
		{
			public Response() { }

			public Response(IList<Lookup<Guid>> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{

					var query = session.Query<Product>();

					if (!message.Id.IsNullOrEmpty())
						query = query.Where(x => message.Id.Contains(x.Id));

					if (message.SupplierId != Guid.Empty)
						query = query.Where(x => x.Suppliers.Any(o => o.Id == message.SupplierId));

					var pairs = query
						.Select(x => new Lookup<Guid>()
						{
							Id = x.Id,
							Name = x.Name
						})
						.OrderBy(x => x.Name)
						.Cacheable()
						.ToList();

					response = new Response(pairs);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
