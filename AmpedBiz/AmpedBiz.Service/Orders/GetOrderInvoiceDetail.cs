﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Orders;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
	public class GetOrderInvoiceDetail
	{
		public class Request : IRequest<Response>
		{
			public Guid Id { get; set; }
		}

		public class Response : Dto.OrderInvoiceDetail { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.QueryOver<Order>()
						.Where(x => x.Id == message.Id)
						.Fetch(x => x.Items).Eager
						.Fetch(x => x.Items.First().Product).Eager
						.Fetch(x => x.Items.First().Product.Inventories).Eager
						.Fetch(x => x.Returns).Eager
						.Fetch(x => x.Returns.First().Product).Eager
						.Fetch(x => x.Returns.First().Product.Inventories).Eager
						.FutureValue();

					var entity = query.Value;
					entity.MapTo(response);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
