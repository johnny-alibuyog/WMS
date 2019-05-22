using AmpedBiz.Core.Common;
using AmpedBiz.Core.Orders;
using AmpedBiz.Core.Returns;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
	public class GetReturnsByProductPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.ReturnsByProductPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = default(IQueryable<Dto.ReturnsByProductPageItem>);

					var includeOrderReturns = false;

					// compose filters
					message.Filter.Compose<bool>("includeOrderReturns", value => includeOrderReturns = value);

					if (includeOrderReturns)
					{
						var query1 = session.Query<TransactionReturnBase>();

						message.Filter.Compose<Guid>("product", value =>
						{
							// we need to do this because where condition from a groupby select messed up the id value (guid)
							query1 = query1.Where(x => x.Product.Id == value);
						});

						message.Filter.Compose<Guid>("branch", value =>
						{
							// we need to do this because where condition from a groupby select messed up the id value (guid)
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Return.Branch.Id == value
								: ((OrderReturn)x).Order.Branch.Id == value
							);
						});

						query = query1
							.Select(x => new
							{
								Id = x.Product.Id,
								BranchName = x is ReturnItem
									? ((ReturnItem)x).Return.Branch.Name
									: ((OrderReturn)x).Order.Branch.Name,
								ProductCode = x.Product.Code,
								ProductName = x.Product.Name,
								QuantityUnit = x.QuantityStandardEquivalent.Unit.Id,
								QuantityValue = x.QuantityStandardEquivalent.Value,
								ReturnedAmount = x.Returned.Amount
							})
							.GroupBy(x => new
							{
								ProductId = x.Id,
								BranchName = x.BranchName
							})
							.Select(x => new Dto.ReturnsByProductPageItem()
							{
								Id = x.Key.ProductId,
								BranchName = x.Key.BranchName,
								ProductCode = x.Max(o => o.ProductCode),
								ProductName = x.Max(o => o.ProductName),
								QuantityUnit = x.Max(o => o.QuantityUnit),
								QuantityValue = x.Sum(o => o.QuantityValue),
								ReturnedAmount = x.Sum(o => o.ReturnedAmount)
							});
					}
					else
					{
						var query1 = session.Query<ReturnItem>();

						message.Filter.Compose<Guid>("product", value =>
						{
							query1 = query1.Where(x => x.Product.Id == value);
						});

						message.Filter.Compose<Guid>("branch", value =>
						{
							query1 = query1.Where(x => x.Return.Branch.Id == value);
						});

						query = query1
							.GroupBy(x => new
							{
								ProductId = x.Product.Id,
								BranchName = x.Return.Branch.Name
							})
							.Select(x => new Dto.ReturnsByProductPageItem()
							{
								Id = x.Key.ProductId,
								BranchName = x.Key.BranchName,
								ProductCode = x.Max(o => o.Product.Code),
								ProductName = x.Max(o => o.Product.Name),
								QuantityUnit = x.Max(o => o.QuantityStandardEquivalent.Unit.Id),
								QuantityValue = x.Sum(o => o.QuantityStandardEquivalent.Value),
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

					message.Sorter.Compose("productName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ProductName)
							: query.OrderByDescending(x => x.ProductName);
					});

					message.Sorter.Compose("quantityValue", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.QuantityValue)
							: query.OrderByDescending(x => x.QuantityValue);
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

					var items = totalItems
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
