using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Orders;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
	public class GetOrderList
	{
		public class Request : IRequest<Response>
		{
			public Guid[] Id { get; set; }
		}

		public class Response : List<Dto.Order>
		{
			public Response() { }

			public Response(List<Dto.Order> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entites = session.QueryOver<Order>()
						.Fetch(x => x.Branch).Eager
						.Fetch(x => x.Customer).Eager
						.Fetch(x => x.Pricing).Eager
						.Fetch(x => x.PaymentType).Eager
						.Fetch(x => x.Shipper).Eager
						.Fetch(x => x.Tax).Eager
						.Fetch(x => x.ShippingFee).Eager
						.Fetch(x => x.Discount).Eager
						.Fetch(x => x.SubTotal).Eager
						.Fetch(x => x.Total).Eager
						.Fetch(x => x.CreatedBy).Eager
						.Fetch(x => x.OrderedBy).Eager
						.Fetch(x => x.RoutedBy).Eager
						.Fetch(x => x.StagedBy).Eager
						.Fetch(x => x.InvoicedBy).Eager
						.Fetch(x => x.PaymentBy).Eager
						.Fetch(x => x.RoutedBy).Eager
						.Fetch(x => x.CompletedBy).Eager
						.Fetch(x => x.CancelledBy).Eager
						.Fetch(x => x.Items).Eager
						.Fetch(x => x.Items.First().Product).Eager
						.Fetch(x => x.Returns).Eager
						.Fetch(x => x.Returns.First().Product).Eager
						.Fetch(x => x.Payments).Eager
						.Fetch(x => x.Payments.First().PaymentBy).Eager
						.Fetch(x => x.Payments.First().PaymentType).Eager
						.TransformUsing(Transformers.DistinctRootEntity)
						.List();

					var dtos = entites.MapTo(default(List<Dto.Order>));

					response = new Response(dtos);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}