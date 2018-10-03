using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
    public class GetReturnsByReasonDetailsPage
	{
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.ReturnsByReasonDetailsPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
					var query = default(IQueryable<Dto.ReturnsByReasonDetailsPageItem>);

					var includeOrderReturns = false;

					message.Filter.Compose<bool>("includeOrderReturns", value => includeOrderReturns = value);

					if (includeOrderReturns)
					{
						var query1 = session.Query<ReturnItemBase>();

						// compose filters
						message.Filter.Compose<string>("reason", value =>
						{
							query1 = query1.Where(x => x.Reason.Id == value);
						});

						message.Filter.Compose<Guid>("branch", value =>
						{
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Return.Branch.Id == value
								: ((OrderReturn)x).Order.Branch.Id == value
							);
						});

						query = query1
							.Select(x => new
							{
								ReasonId = x.Reason.Id,
								BranchName = x is ReturnItem
									? ((ReturnItem)x).Return.Branch.Name
									: ((OrderReturn)x).Order.Branch.Name,
								ReasonName = x.Reason.Name,
								ReturnedOn = x is ReturnItem
									? ((ReturnItem)x).Return.ReturnedOn.Value.Date
									: ((OrderReturn)x).ReturnedOn.Value.Date,
								ReturnedAmount = x.Returned.Amount
							})
							.GroupBy(x => new
							{
								ReasonId = x.ReasonId,
								BranchName = x.BranchName,
								ReturnedOn = x.ReturnedOn,
							})
							.Select(x => new Dto.ReturnsByReasonDetailsPageItem()
							{
								Id = x.Key.ReasonId,
								BranchName = x.Key.BranchName,
								ReturnedOn = x.Key.ReturnedOn,
								ReasonName = x.Max(o => o.ReasonName),
								ReturnedAmount = x.Sum(o => o.ReturnedAmount)
							});
					}
					else
					{
						var query1 = session.Query<ReturnItem>();

						// compose filters
						message.Filter.Compose<string>("reason", value =>
						{
							query1 = query1.Where(x => x.Reason.Id == value);
						});

						message.Filter.Compose<Guid>("branch", value =>
						{
							query1 = query1.Where(x => x.Return.Branch.Id == value);
						});

						query = query1
							.GroupBy(x => new
							{
								ReasonId = x.Reason.Id,
								BranchName = x.Return.Branch.Name,
								ReturnedOn = x.Return.ReturnedOn.Value.Date
							})
							.Select(x => new Dto.ReturnsByReasonDetailsPageItem()
							{
								Id = x.Key.ReasonId,
								BranchName = x.Key.BranchName,
								ReturnedOn = x.Key.ReturnedOn,
								ReasonName = x.Max(o => o.Reason.Name),
								ReturnedAmount = x.Sum(o => o.Returned.Amount)
							});
					}

					// compose sort
					message.Sorter.Compose("branchName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.BranchName)
							: query.OrderByDescending(x => x.BranchName);
					});

					message.Sorter.Compose("reasonName", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReasonName)
                            : query.OrderByDescending(x => x.ReasonName);
                    });

					message.Sorter.Compose("returnedOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ReturnedOn)
							: query.OrderByDescending(x => x.ReturnedOn);
					});

					message.Sorter.Compose("returnedAmount", direction =>
                    {
                        query = direction == SortDirection.Ascending
                            ? query.OrderBy(x => x.ReturnedAmount)
                            : query.OrderByDescending(x => x.ReturnedAmount);
                    });

					// TODO: this is not performant, this is just a work around on groupby count issue of nhibernate. find a solution soon
					var totalItems = query.ToList();

					var count = totalItems.Count;

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(count);

					var items = query
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToList();

					response = new Response()
					{
						Count = count,
						Items = items
					};

					//var itemsFuture = query
					//    .Skip(message.Pager.SkipCount)
					//    .Take(message.Pager.Size)
					//    .ToFuture();

					//var countFuture = query
					//    .ToFutureValue(x => x.Count());

					//response = new Response()
					//{
					//    Count = countFuture.Value,
					//    Items = itemsFuture.ToList()
					//};

					transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
