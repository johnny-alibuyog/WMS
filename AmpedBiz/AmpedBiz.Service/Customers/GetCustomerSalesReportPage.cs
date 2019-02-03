using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Orders;
using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
	// TODO: WARNING: This is realy BAD because it fetch all data and do the pagination in memory! Optimize this sometime soon
	public class GetCustomerSalesReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.CustomerSalesReportPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				if (message.Filter == null)
					message.Filter = new Filter();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = (this.GetOrderSales(message, session))
						.Union(this.GetPointOfSaleSales(message, session));

					// compose order
					message.Sorter.Compose("paidOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.PaidOn)
							: query.OrderByDescending(x => x.PaidOn);
					});

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

					message.Sorter.Compose("invoice", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.InvoiceNumber)
							: query.OrderByDescending(x => x.InvoiceNumber);
					});

					message.Sorter.Compose("status", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Status)
							: query.OrderByDescending(x => x.Status);
					});

					message.Sorter.Compose("totalAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.TotalAmount)
							: query.OrderByDescending(x => x.TotalAmount);
					});

					message.Sorter.Compose("balanceAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.BalanceAmount)
							: query.OrderByDescending(x => x.BalanceAmount);
					});

					var totalItems = query.ToList();

					var totalCount = totalItems.Count;

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(totalCount);

					var pagedItems = totalItems
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToList();

					response = new Response()
					{
						Count = totalCount,
						Items = pagedItems
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}

			private List<Dto.CustomerSalesReportPageItem> GetOrderSales(Request message, ISession session)
			{
				var query = session.Query<OrderPayment>();

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Order.Branch.Id == value);
				});

				message.Filter.Compose<Guid>("customerId", value =>
				{
					query = query.Where(x => x.Order.Customer.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.PaidOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PaidOn <= value.EndOfDay());
				});

				var items = query
					.GroupBy(x => new
					{
						BranchName = x.Order.Branch.Name,
						CustomerName = x.Order.Customer.Name,
						InvoiceNumber = x.Order.InvoiceNumber,
					})
					.Select(x => new Dto.CustomerSalesReportPageItem()
					{
						PaidOn = x.Max(o => o.PaidOn),
						BranchName = x.Key.BranchName,
						CustomerName = x.Key.CustomerName,
						InvoiceNumber = x.Key.InvoiceNumber,
						Status = x.Min(o => o.Order.Status).ToString(),
						TotalAmount = x.Max(o => o.Order.Total.Amount),
						PaidAmount = x.Sum(o => o.Payment.Amount),
						BalanceAmount = x.Sum(o => o.Balance.Amount)
					})
					.ToList();

				return items;
			}

			private List<Dto.CustomerSalesReportPageItem> GetPointOfSaleSales(Request message, ISession session)
			{
				var query = session.Query<PointOfSale>();

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Branch.Id == value);
				});

				message.Filter.Compose<Guid>("customerId", value =>
				{
					query = query.Where(x => x.Customer.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.TenderedOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.TenderedOn <= value.EndOfDay());
				});

				var items = query
					.Select(x => new Dto.CustomerSalesReportPageItem()
					{
						PaidOn = x.TenderedOn,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						InvoiceNumber = x.InvoiceNumber,
						Status = "Point of Sale",
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount,
						BalanceAmount = 0
					})
					.ToList();

				return items;
			}
		}
	}

	/*
	public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				if (message.Filter == null)
					message.Filter = new Filter();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = (this.GetOrderSales(message, session))
						.Union(this.GetPointOfSaleSales(message, session));

					// compose order
					message.Sorter.Compose("paidOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.PaidOn)
							: query.OrderByDescending(x => x.PaidOn);
					});

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

					message.Sorter.Compose("invoice", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.InvoiceNumber)
							: query.OrderByDescending(x => x.InvoiceNumber);
					});

					message.Sorter.Compose("status", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Status)
							: query.OrderByDescending(x => x.Status);
					});

					message.Sorter.Compose("totalAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.TotalAmount)
							: query.OrderByDescending(x => x.TotalAmount);
					});

					message.Sorter.Compose("balanceAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.TotalAmount - x.PaidAmount)
							: query.OrderByDescending(x => x.TotalAmount - x.PaidAmount);
					});

					var totalItems = query.ToList();

					var totalCount = totalItems.Count;

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(totalCount);

					var pagedItems = totalItems
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToList();

					response = new Response()
					{
						Count = totalCount,
						Items = pagedItems
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}

			private List<Dto.CustomerSalesReportPageItem> GetOrderSales(Request message, ISession session)
			{
				var query = session.Query<OrderPayment>();

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Order.Branch.Id == value);
				});

				message.Filter.Compose<Guid>("customerId", value =>
				{
					query = query.Where(x => x.Order.Customer.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.PaidOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PaidOn <= value.EndOfDay());
				});

				var groupedQuery = query
					.GroupBy(x => new
					{
						BranchName = x.Order.Branch.Name,
						CustomerName = x.Order.Customer.Name,
						InvoiceNumber = x.Order.InvoiceNumber,
					})
					.Select(x => new Dto.CustomerSalesReportPageItem()
					{
						PaidOn = x.Max(o => o.PaidOn),
						BranchName = x.Key.BranchName,
						CustomerName = x.Key.CustomerName,
						InvoiceNumber = x.Key.InvoiceNumber,
						Status = x.Min(o => o.Order.Status).ToString(),
						TotalAmount = x.Max(o => o.Order.Total.Amount),
						PaidAmount = x.Sum(o => o.Payment.Amount)
					});

				// compose order
				message.Sorter.Compose("paidOn", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.PaidOn)
						: groupedQuery.OrderByDescending(x => x.PaidOn);
				});

				message.Sorter.Compose("branchName", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.BranchName)
						: groupedQuery.OrderByDescending(x => x.BranchName);
				});

				message.Sorter.Compose("customerName", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.CustomerName)
						: groupedQuery.OrderByDescending(x => x.CustomerName);
				});

				message.Sorter.Compose("invoice", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.InvoiceNumber)
						: groupedQuery.OrderByDescending(x => x.InvoiceNumber);
				});

				message.Sorter.Compose("status", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.Status)
						: groupedQuery.OrderByDescending(x => x.Status);
				});

				message.Sorter.Compose("totalAmount", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.TotalAmount)
						: groupedQuery.OrderByDescending(x => x.TotalAmount);
				});

				message.Sorter.Compose("balanceAmount", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.TotalAmount - x.PaidAmount)
						: groupedQuery.OrderByDescending(x => x.TotalAmount - x.PaidAmount);
				});

				var countFuture = query.GroupByCount(x => new
				{
					BranchName = x.Order.Branch.Name,
					CustomerName = x.Order.Customer.Name,
					InvoiceNumber = x.Order.InvoiceNumber,
				});

				if (message.Pager.IsPaged() != true)
					message.Pager.RetrieveAll(countFuture.Value);

				var items = groupedQuery
					.Skip(message.Pager.SkipCount)
					.Take(message.Pager.Size)
					.ToList();

				return items;
			}

			private List<Dto.CustomerSalesReportPageItem> GetPointOfSaleSales(Request message, ISession session)
			{
				var query = session.Query<PointOfSale>();

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Branch.Id == value);
				});

				message.Filter.Compose<Guid>("customerId", value =>
				{
					query = query.Where(x => x.Customer.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.TenderedOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.TenderedOn <= value.EndOfDay());
				});

				var selectQuery = query
					.Select(x => new Dto.CustomerSalesReportPageItem()
					{
						PaidOn = x.TenderedOn,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						InvoiceNumber = x.InvoiceNumber,
						Status = "Point of Sale",
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount
					});

				// compose order
				message.Sorter.Compose("paidOn", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.PaidOn)
						: selectQuery.OrderByDescending(x => x.PaidOn);
				});

				message.Sorter.Compose("branchName", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.BranchName)
						: selectQuery.OrderByDescending(x => x.BranchName);
				});

				message.Sorter.Compose("customerName", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.CustomerName)
						: selectQuery.OrderByDescending(x => x.CustomerName);
				});

				message.Sorter.Compose("invoice", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.InvoiceNumber)
						: selectQuery.OrderByDescending(x => x.InvoiceNumber);
				});

				message.Sorter.Compose("status", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.Status)
						: selectQuery.OrderByDescending(x => x.Status);
				});

				message.Sorter.Compose("totalAmount", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.TotalAmount)
						: selectQuery.OrderByDescending(x => x.TotalAmount);
				});

				message.Sorter.Compose("balanceAmount", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.TotalAmount - x.PaidAmount)
						: selectQuery.OrderByDescending(x => x.TotalAmount - x.PaidAmount);
				});

				var countFuture = selectQuery.ToFutureValue(x => x.Count());

				if (message.Pager.IsPaged() != true)
					message.Pager.RetrieveAll(countFuture.Value);

				var items = selectQuery
					.Skip(message.Pager.SkipCount)
					.Take(message.Pager.Size)
					.ToList();

				return items;
			}
		}
	}
 
	 */
	//public class GetCustomerSalesReportPage1
	//{
	//    public class Request : PageRequest, IRequest<Response> { }

	//    public class Response : PageResponse<Dto.CustomerSalesReportPageItem> { }

	//    public class Handler : RequestHandlerBase<Request, Response>
	//    {
	//        public override Response Execute(Request message)
	//        {
	//            var response = new Response();

	//            if (message.Filter == null)
	//                message.Filter = new Filter();

	//            using (var session = SessionFactory.RetrieveSharedSession(Context))
	//            using (var transaction = session.BeginTransaction())
	//            {
	//                var query = session.Query<Order>()
	//                    .Where(x => x.Status == OrderStatus.Completed);

	//                // compose filter
	//                message.Filter.Compose<Guid>("branchId", value =>
	//                {
	//                    query = query.Where(x => x.Branch.Id == value);
	//                });

	//                message.Filter.Compose<Guid>("customerId", value =>
	//                {
	//                    query = query.Where(x => x.Customer.Id == value);
	//                });

	//                message.Filter.Compose<DateTime>("fromDate", value =>
	//                {
	//                    query = query.Where(x => x.CompletedOn >= value.StartOfDay());
	//                });

	//                message.Filter.Compose<DateTime>("toDate", value =>
	//                {
	//                    query = query.Where(x => x.CompletedOn <= value.EndOfDay());
	//                });

	//                // compose order
	//                message.Sorter.Compose("completedOn", direction =>
	//                {
	//                    query = direction == SortDirection.Ascending
	//                        ? query.OrderBy(x => x.CompletedOn)
	//                        : query.OrderByDescending(x => x.CompletedOn);
	//                });

	//                message.Sorter.Compose("branchName", direction =>
	//                {
	//                    query = direction == SortDirection.Ascending
	//                        ? query.OrderBy(x => x.Branch.Name)
	//                        : query.OrderByDescending(x => x.Branch.Name);
	//                });

	//                message.Sorter.Compose("customerName", direction =>
	//                {
	//                    query = direction == SortDirection.Ascending
	//                        ? query.OrderBy(x => x.Customer.Name)
	//                        : query.OrderByDescending(x => x.Customer.Name);
	//                });

	//                message.Sorter.Compose("invoice", direction =>
	//                {
	//                    query = direction == SortDirection.Ascending
	//                        ? query.OrderBy(x => x.InvoiceNumber)
	//                        : query.OrderByDescending(x => x.InvoiceNumber);
	//                });

	//                message.Sorter.Compose("totalAmount", direction =>
	//                {
	//                    query = direction == SortDirection.Ascending
	//                        ? query.OrderBy(x => x.Total.Amount)
	//                        : query.OrderByDescending(x => x.Total.Amount);
	//                });

	//                message.Sorter.Compose("balanceAmount", direction =>
	//                {
	//                    query = direction == SortDirection.Ascending
	//                        ? query.OrderBy(x => x.Total.Amount - x.Paid.Amount)
	//                        : query.OrderByDescending(x => x.Total.Amount - x.Paid.Amount);
	//                });

	//                var countFuture = query
	//                    .ToFutureValue(x => x.Count());

	//                if (message.Pager.IsPaged() != true)
	//                    message.Pager.RetrieveAll(countFuture.Value);

	//                var itemsFuture = query
	//                    .Select(x => new Dto.CustomerSalesReportPageItem()
	//                    {
	//                        PaidOn = x.CompletedOn,
	//                        BranchName = x.Branch.Name,
	//                        CustomerName = x.Customer.Name,
	//                        InvoiceNumber = x.InvoiceNumber,
	//                        TotalAmount = x.Total.Amount,
	//                        PaidAmount = x.Paid.Amount,
	//                    })
	//                    .Skip(message.Pager.SkipCount)
	//                    .Take(message.Pager.Size)
	//                    .ToFuture();

	//                response = new Response()
	//                {
	//                    Count = countFuture.Value,
	//                    Items = itemsFuture.ToList()
	//                };

	//                transaction.Commit();

	//                SessionFactory.ReleaseSharedSession();
	//            }

	//            return response;
	//        }
	//    }
	//}
}
