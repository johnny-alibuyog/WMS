using AmpedBiz.Common.Extentions;
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
	public class GetReturnDetailsReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.ReturnsDetailsReportPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = default(IQueryable<Dto.ReturnsDetailsReportPageItem>);

					var includeOrderReturns = false;

					// compose filters
					message.Filter.Compose<bool>("includeOrderReturns", value => includeOrderReturns = value);

					if (includeOrderReturns)
					{
						var query1 = session.Query<TransactionReturnBase>();

						message.Filter.Compose<Guid>("customer", value =>
						{
							// we need to do this because where condition from a groupby select messed up the id value (guid)
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Return.Customer.Id == value
								: ((OrderReturn)x).Order.Customer.Id == value
							);
						});

						message.Filter.Compose<Guid>("product", value =>
						{
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Product.Id == value
								: ((OrderReturn)x).Product.Id == value
							);
						});

						message.Filter.Compose<Guid>("branch", value =>
						{
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Return.Branch.Id == value
								: ((OrderReturn)x).Order.Branch.Id == value
							);
						});

						message.Filter.Compose<DateTime>("fromDate", value =>
						{
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Return.ReturnedOn >= value.StartOfDay()
								: ((OrderReturn)x).ReturnedOn >= value.StartOfDay()
							);
						});

						message.Filter.Compose<DateTime>("toDate", value =>
						{
							query1 = query1.Where(x => x is ReturnItem
								? ((ReturnItem)x).Return.ReturnedOn <= value.EndOfDay()
								: ((OrderReturn)x).ReturnedOn <= value.EndOfDay()
							);
						});

						query = query1
							.Select(x => new Dto.ReturnsDetailsReportPageItem()
							{
								Id = x is ReturnItem
									? ((ReturnItem)x).Id
									: ((OrderReturn)x).Id,
								BranchName = x is ReturnItem
									? ((ReturnItem)x).Return.Branch.Name
									: ((OrderReturn)x).Order.Branch.Name,
								CustomerName = x is ReturnItem
									? ((ReturnItem)x).Return.Customer.Name
									: ((OrderReturn)x).Order.Customer.Name,
								ProductName = x is ReturnItem
									? ((ReturnItem)x).Product.Name
									: ((OrderReturn)x).Product.Name,
								ReasonName = x is ReturnItem
									? ((ReturnItem)x).Reason.Name
									: ((OrderReturn)x).Reason.Name,
								ReturnedByName = x is ReturnItem
									? ((ReturnItem)x).Return.ReturnedBy.Person.FirstName + " " +
									  ((ReturnItem)x).Return.ReturnedBy.Person.LastName
									: ((OrderReturn)x).ReturnedBy.Person.FirstName + " " +
									  ((OrderReturn)x).ReturnedBy.Person.LastName,
								ReturnedOn = x is ReturnItem
									? ((ReturnItem)x).Return.ReturnedOn.Value.Date
									: ((OrderReturn)x).ReturnedOn.Value.Date,
								QuantityValue = x is ReturnItem
									? ((ReturnItem)x).Quantity.Value
									: ((OrderReturn)x).Quantity.Value,
								QuantityUnitId = x is ReturnItem
									? ((ReturnItem)x).Quantity.Unit.Id
									: ((OrderReturn)x).Quantity.Unit.Id,
								ReturnedAmount = x is ReturnItem
									? ((ReturnItem)x).Returned.Amount
									: ((OrderReturn)x).Returned.Amount,
							});
					}
					else
					{
						var query1 = session.Query<ReturnItem>();

						message.Filter.Compose<Guid>("customer", value =>
						{
							query1 = query1.Where(x => x.Return.Customer.Id == value);
						});

						message.Filter.Compose<Guid>("product", value =>
						{
							query1 = query1.Where(x => x.Product.Id == value);
						});

						message.Filter.Compose<Guid>("branch", value =>
						{
							query1 = query1.Where(x => x.Return.Branch.Id == value);
						});

						message.Filter.Compose<DateTime>("fromDate", value =>
						{
							query1 = query1.Where(x => x.Return.ReturnedOn >= value.StartOfDay());
						});

						message.Filter.Compose<DateTime>("toDate", value =>
						{
							query1 = query1.Where(x => x.Return.ReturnedOn <= value.StartOfDay());
						});

						query = query1
							.Select(x => new Dto.ReturnsDetailsReportPageItem()
							{
								Id = x.Id,
								BranchName = x.Return.Branch.Name,
								CustomerName = x.Return.Customer.Name,
								ProductName = x.Product.Name,
								ReasonName = x.Reason.Name,
								ReturnedByName =
									x.Return.ReturnedBy.Person.FirstName + " " +
									x.Return.ReturnedBy.Person.LastName,
								ReturnedOn = x.Return.ReturnedOn.Value.Date,
								QuantityValue = x.Quantity.Value,
								QuantityUnitId = x.Quantity.Unit.Id,
								ReturnedAmount = x.Returned.Amount
							});
					}

					// compose sort
					message.Sorter.Compose("branchName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.BranchName)
							: query.OrderByDescending(x => x.BranchName);
					});

					message.Sorter.Compose("customerName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.CustomerName)
							: query.OrderByDescending(x => x.CustomerName);
					});

					message.Sorter.Compose("productName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ProductName)
							: query.OrderByDescending(x => x.ProductName);
					});

					message.Sorter.Compose("returnedOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ReturnedOn)
							: query.OrderByDescending(x => x.ReturnedOn);
					});

					message.Sorter.Compose("returnedByName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ReturnedByName)
							: query.OrderByDescending(x => x.ReturnedByName);
					});

					message.Sorter.Compose("reasonName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ReasonName)
							: query.OrderByDescending(x => x.ReasonName);
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

					var countFuture = query
						.ToFutureValue(x => x.Count());

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(countFuture.Value);

					var itemsFuture = query
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
