using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.UnitOfMeasures
{
	public class GetUnitOfMeasurePage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.UnitOfMeasurePageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.Query<UnitOfMeasure>();

					// compose filters
					message.Filter.Compose<string>("code", value =>
					{
						query = query.Where(x => x.Id.ToLower().Contains(value.ToLower()));
					});

					message.Filter.Compose<string>("name", value =>
					{
						query = query.Where(x => x.Name.ToLower().Contains(value.ToLower()));
					});

					// compose sort
					message.Sorter.Compose("code", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Id)
							: query.OrderByDescending(x => x.Id);
					});

					message.Sorter.Compose("name", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Name)
							: query.OrderByDescending(x => x.Name);
					});

					var countFuture = query
						.ToFutureValue(x => x.Count());

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(countFuture.Value);

					var itemsFuture = query
						.Select(x => new Dto.UnitOfMeasurePageItem()
						{
							Id = x.Id,
							Name = x.Name,
						})
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToFuture();

					response = new Response()
					{
						Count = countFuture.Value,
						Items = itemsFuture.ToList()
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
