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
	public class GetCustomerPaymentsReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.CustomerPaymentReportPageItem> { }

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
					var orderPaymentsItems = this.GetOrderPayments(message, session);
					var pointOfSalePaymentsItems = this.GetPointOfSalePayments(message, session);

					var query = (orderPaymentsItems)
						.Union(pointOfSalePaymentsItems);

					// compose order
					message.Sorter.Compose("paymentOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.PaymentOn)
							: query.OrderByDescending(x => x.PaymentOn);
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

					message.Sorter.Compose("paymentTypeName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.PaymentTypeName)
							: query.OrderByDescending(x => x.PaymentTypeName);
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
						Items = pagedItems.ToList()
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}

			private List<Dto.CustomerPaymentReportPageItem> GetOrderPayments(Request message, ISession session)
			{
				var query = session.Query<Order>()
					.Where(x => x.Paid != null);

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Branch.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.PaymentOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PaymentOn <= value.EndOfDay());
				});

				var items = query
					.Select(x => new Dto.CustomerPaymentReportPageItem()
					{
						PaymentOn = x.PaymentOn,
						InvoiceNumber = x.InvoiceNumber,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						PaymentTypeName = x.PaymentType.Name,
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount,
						BalanceAmount = x.Balance.Amount
					})
					.ToList();

				return items;
			}

			private List<Dto.CustomerPaymentReportPageItem> GetPointOfSalePayments(Request message, ISession session)
			{
				var query = session.Query<PointOfSale>();

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Branch.Id == value);
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
					.Select(x => new Dto.CustomerPaymentReportPageItem()
					{
						PaymentOn = x.TenderedOn,
						InvoiceNumber = x.InvoiceNumber,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						PaymentTypeName = x.PaymentType.Name,
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount,
						BalanceAmount = x.Total.Amount - x.Paid.Amount
					})
					.ToList();

				return items;
			}
		}
	}


	/*
	public class GetCustomerPaymentsReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.CustomerPaymentReportPageItem> { }

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
					var orderPaymentsPage = this.GetOrderPayments(message, session);
					var pointOfSalePaymentsPage = this.GetPointOfSalePayments(message, session);

					var query = (orderPaymentsPage.Items)
						.Union(pointOfSalePaymentsPage.Items);

					// compose order
					message.Sorter.Compose("paymentOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.PaymentOn)
							: query.OrderByDescending(x => x.PaymentOn);
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

					message.Sorter.Compose("paymentTypeName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.PaymentTypeName)
							: query.OrderByDescending(x => x.PaymentTypeName);
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

					var totalCount = orderPaymentsPage.Count + pointOfSalePaymentsPage.Count;

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(totalCount ?? 0);

					var pagedItems = totalItems
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToList();

					response = new Response()
					{
						Count = totalCount,
						Items = pagedItems.ToList()
					};

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}

			private Response GetOrderPayments(Request message, ISession session)
			{
				var query = session.Query<Order>()
					.Where(x => x.Paid != null);

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Branch.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.PaymentOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PaymentOn <= value.EndOfDay());
				});

				// compose order
				message.Sorter.Compose("paymentOn", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.PaymentOn)
						: query.OrderByDescending(x => x.PaymentOn);
				});

				message.Sorter.Compose("branchName", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Branch.Name)
						: query.OrderByDescending(x => x.Branch.Name);
				});

				message.Sorter.Compose("customerName", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Customer.Name)
						: query.OrderByDescending(x => x.Customer.Name);
				});

				message.Sorter.Compose("invoice", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.InvoiceNumber)
						: query.OrderByDescending(x => x.InvoiceNumber);
				});

				message.Sorter.Compose("paymentTypeName", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.PaymentType.Name)
						: query.OrderByDescending(x => x.PaymentType.Name);
				});

				message.Sorter.Compose("totalAmount", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Total.Amount)
						: query.OrderByDescending(x => x.Total.Amount);
				});

				message.Sorter.Compose("balanceAmount", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Balance.Amount)
						: query.OrderByDescending(x => x.Balance.Amount);
				});

				var countFuture = query
					.ToFutureValue(x => x.Count());

				if (message.Pager.IsPaged() != true)
					message.Pager.RetrieveAll(countFuture.Value);

				var itemsFuture = query
					.Select(x => new Dto.CustomerPaymentReportPageItem()
					{
						PaymentOn = x.PaymentOn,
						InvoiceNumber = x.InvoiceNumber,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						PaymentTypeName = x.PaymentType.Name,
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount,
						BalanceAmount = x.Balance.Amount
					})
					.Skip(message.Pager.SkipCount)
					.Take(message.Pager.Size)
					.ToFuture();

				var items = itemsFuture.ToList();

				return new Response()
				{
					Count = countFuture.Value,
					Items = itemsFuture.ToList()
				};
			}

			private Response GetPointOfSalePayments(Request message, ISession session)
			{
				var query = session.Query<PointOfSale>();

				// compose filter
				message.Filter.Compose<Guid>("branchId", value =>
				{
					query = query.Where(x => x.Branch.Id == value);
				});

				message.Filter.Compose<DateTime>("fromDate", value =>
				{
					query = query.Where(x => x.TenderedOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.TenderedOn <= value.EndOfDay());
				});

				// compose order
				message.Sorter.Compose("paymentOn", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.TenderedOn)
						: query.OrderByDescending(x => x.TenderedOn);
				});

				message.Sorter.Compose("branchName", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Branch.Name)
						: query.OrderByDescending(x => x.Branch.Name);
				});

				message.Sorter.Compose("customerName", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Customer.Name)
						: query.OrderByDescending(x => x.Customer.Name);
				});

				message.Sorter.Compose("invoice", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.InvoiceNumber)
						: query.OrderByDescending(x => x.InvoiceNumber);
				});

				message.Sorter.Compose("paymentTypeName", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.PaymentType.Name)
						: query.OrderByDescending(x => x.PaymentType.Name);
				});

				message.Sorter.Compose("totalAmount", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Total.Amount)
						: query.OrderByDescending(x => x.Total.Amount);
				});

				message.Sorter.Compose("balanceAmount", direction =>
				{
					query = direction == SortDirection.Ascending
						? query.OrderBy(x => x.Total.Amount - x.Paid.Amount)
						: query.OrderByDescending(x => x.Total.Amount - x.Paid.Amount);
				});

				var countFuture = query
					.ToFutureValue(x => x.Count());

				if (message.Pager.IsPaged() != true)
					message.Pager.RetrieveAll(countFuture.Value);

				var itemsFuture = query
					.Select(x => new Dto.CustomerPaymentReportPageItem()
					{
						PaymentOn = x.TenderedOn,
						InvoiceNumber = x.InvoiceNumber,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						PaymentTypeName = x.PaymentType.Name,
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount,
						BalanceAmount = x.Total.Amount - x.Paid.Amount
					})
					.Skip(message.Pager.SkipCount)
					.Take(message.Pager.Size)
					.ToFuture();

				var items = itemsFuture.ToList();

				return new Response()
				{
					Count = countFuture.Value,
					Items = itemsFuture.ToList()
				};
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
	//                        PaymentOn = x.CompletedOn,
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
