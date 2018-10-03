using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
	public class GetReturnsByCustomerReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.ReturnsByCustomerReportPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = default(IQueryable<Dto.ReturnsByCustomerReportPageItem>);

					var includeOrderReturns = false;
					var customerId = Guid.Empty;
					var branchId = Guid.Empty;
					var fromDate = (DateTime?)null;
					var toDate = (DateTime?)null;

					// compose filters
					message.Filter.Compose<bool>("includeOrderReturns", value => includeOrderReturns = value);
					message.Filter.Compose<Guid>("customer", value => customerId = value);
					message.Filter.Compose<Guid>("branch", value => branchId = value);

					var query1 = session.Query<ReturnItemBase>();

					if (customerId != Guid.Empty)
					{
						// we need to do this because where condition from a groupby select messed up the id value (guid)
						query1 = query1.Where(x => x is ReturnItem
							? ((ReturnItem)x).Return.Customer.Id == customerId 
							: ((OrderReturn)x).Order.Customer.Id == customerId
						);
					}

					if (branchId != Guid.Empty)
					{
						// we need to do this because where condition from a groupby select messed up the id value (guid)
						query1 = query1.Where(x => x is ReturnItem
							? ((ReturnItem)x).Return.Branch.Id == customerId
							: ((OrderReturn)x).Order.Branch.Id == customerId
						);
					}

					if (fromDate != null)
					{
						// we need to do this because where condition from a groupby select messed up the id value (guid)
						query1 = query1.Where(x => x is ReturnItem
							? ((ReturnItem)x).Return.ReturnedOn.Value.Date >= fromDate
							: ((OrderReturn)x).ReturnedOn >= fromDate
						);
					}

					if (toDate != null)
					{
						// we need to do this because where condition from a groupby select messed up the id value (guid)
						query1 = query1.Where(x => x is ReturnItem
							? ((ReturnItem)x).Return.ReturnedOn.Value.Date <= toDate
							: ((OrderReturn)x).ReturnedOn <= toDate
						);
					}

					query = query1
						.Select(x => new
						{
							CustomerId = x is ReturnItem
								? ((ReturnItem)x).Return.Customer.Id
								: ((OrderReturn)x).Order.Customer.Id,
							CustomerCode = x is ReturnItem
								? ((ReturnItem)x).Return.Customer.Code
								: ((OrderReturn)x).Order.Customer.Code,
							CustomerName = x is ReturnItem
								? ((ReturnItem)x).Return.Customer.Name
								: ((OrderReturn)x).Order.Customer.Name,
							ReturnedAmount = x is ReturnItem
								? ((ReturnItem)x).Returned.Amount
								: ((OrderReturn)x).Returned.Amount,
						})
						.GroupBy(x => x.CustomerId)
						.Select(x => new Dto.ReturnsByCustomerReportPageItem()
						{
							Id = x.Key,
							CustomerCode = x.Min(o => o.CustomerCode),
							CustomerName = x.Max(o => o.CustomerName),
							ReturnedAmount = x.Sum(o => o.ReturnedAmount)
						});
	
					// compose sort
					message.Sorter.Compose("customer", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.CustomerName)
							: query.OrderByDescending(x => x.CustomerName);
					});

					message.Sorter.Compose("returnedAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.ReturnedAmount)
							: query.OrderByDescending(x => x.ReturnedAmount);
					});

					// TODO: this is not performant, this is just a work around on groupby count issue of nhibernate. find a solution soon
					var totalItems = query.ToList();

					// NOTE: GroupBy(x => x.CustomerId) Messed up the values of the id (conveted to BINARY(16)) we need to fetch the correct values
					var customers = session.Query<Customer>().Cacheable().ToList();
					totalItems.ForEach(x => this.FillCorrectId(x, customers));

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

					//var countFuture = query
					//	.ToFutureValue(x => x.Count());

					//if (message.Pager.IsPaged() != true)
					//	message.Pager.RetrieveAll(countFuture.Value);

					//var itemsFuture = query
					//	.Skip(message.Pager.SkipCount)
					//	.Take(message.Pager.Size)
					//	.ToFuture();

					//response = new Response()
					//{
					//	Count = countFuture.Value,
					//	Items = itemsFuture.ToList()
					//};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}

			private void FillCorrectId(Dto.ReturnsByCustomerPageItem instance, IReadOnlyList<Customer> customers)
			{
				var customer = customers.FirstOrDefault(o =>
					o.Code == instance.CustomerCode &&
					o.Name == instance.CustomerName
				);

				if (customer != null)
				{
					instance.Id = customer.Id;
				}
			}
		}
	}
}
