using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Linq;

namespace AmpedBiz.Service.PointOfSales
{
	public class GetPointOfSale
	{
		public class Request : IRequest<Response>
		{
			public Guid Id { get; set; }

			public Request() : this(default(Guid)) { }

			public Request(Guid id) => this.Id = id;
		}

		public class Response : Dto.PointOfSale { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.QueryOver<PointOfSale>()
						.Where(x => x.Id == message.Id);

					query
						.Fetch(x => x.Branch).Eager
						.Fetch(x => x.Customer).Eager
						.Fetch(x => x.Pricing).Eager
						.Fetch(x => x.PaymentType).Eager
						.Fetch(x => x.TenderedBy).Eager
						.Fetch(x => x.Discount).Eager
						.Fetch(x => x.SubTotal).Eager
						.Fetch(x => x.Total).Eager
						.Fetch(x => x.CreatedBy).Eager
						.Future();

					query
						.Fetch(x => x.Items).Eager
						.Fetch(x => x.Items.First().Product).Eager
						.Future();

					query
						.Fetch(x => x.Payments).Eager
						.Fetch(x => x.Payments.First().PaymentType).Eager
						.Future();

					var entity = query.SingleOrDefault();

					entity.EnsureExistence($"Order with id {message.Id} does not exists.");

					entity.MapTo(response);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
