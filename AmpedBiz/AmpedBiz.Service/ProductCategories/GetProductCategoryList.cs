﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.ProductCategories
{
	public class GetProductCategoryList
	{
		public class Request : IRequest<Response>
		{
			public string[] Id { get; set; }
		}

		public class Response : List<Dto.ProductCategory>
		{
			public Response() { }

			public Response(List<Dto.ProductCategory> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entites = session.Query<ProductCategory>().Cacheable().ToList();
					var dtos = entites.MapTo(default(List<Dto.ProductCategory>));

					response = new Response(dtos);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
