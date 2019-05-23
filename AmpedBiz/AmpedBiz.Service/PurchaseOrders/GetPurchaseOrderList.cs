using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
	public class GetPurchaseOrderList
	{
		public class Request : IRequest<Response>
		{
			public Guid[] Id { get; set; }
		}

		public class Response : List<Dto.PurchaseOrder>
		{
			public Response() { }

			public Response(List<Dto.PurchaseOrder> items) : base(items) { }
		}

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entites = session.QueryOver<PurchaseOrder>()
						.Fetch(x => x.Tax).Eager
						.Fetch(x => x.ShippingFee).Eager
						.Fetch(x => x.Shipper).Eager
						.Fetch(x => x.Supplier).Eager
						.Fetch(x => x.PaymentType).Eager
						.Fetch(x => x.Paid).Eager
						.Fetch(x => x.Balance).Eager
						.Fetch(x => x.SubTotal).Eager
						.Fetch(x => x.Total).Eager
						.Fetch(x => x.CreatedBy).Eager
						.Fetch(x => x.SubmittedBy).Eager
						.Fetch(x => x.ApprovedBy).Eager
						.Fetch(x => x.PaymentBy).Eager
						.Fetch(x => x.ReceivedBy).Eager
						.Fetch(x => x.CompletedBy).Eager
						.Fetch(x => x.CancelledBy).Eager
						.Fetch(x => x.Items).Eager
						.Fetch(x => x.Items.First().Product).Eager
						.Fetch(x => x.Items.First().Product.Inventories).Eager
						.Fetch(x => x.Payments).Eager
						.Fetch(x => x.Payments.First().PaymentBy).Eager
						.Fetch(x => x.Receipts).Eager
						.Fetch(x => x.Receipts.First().Product).Eager
						.Fetch(x => x.Receipts.First().Product.Inventories).Eager
						.TransformUsing(Transformers.DistinctRootEntity)
						.List();

					var dtos = entites.MapTo(default(List<Dto.PurchaseOrder>));

					response = new Response(dtos);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}