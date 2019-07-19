using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
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
    /*
    // https://stackoverflow.com/questions/12715145/hql-on-subclass-property
    // https://stackoverflow.com/questions/11496843/nhibernate-class-equivalent-in-queryover
    // https://stackoverflow.com/questions/5566963/fluent-nhibernate-hql-assigning-multiple-class-types-to-multiple-table-select-query
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
                    var itemsHql = $@"
                        select 
                            tb.PaymentOn as PaymentOn,
                            tb.Branch.Name as BranchName,
                            tb.Customer.Name as CustomerName,
                            tb.InvoiceNumber as InvoiceNumber,
                            tb.Status as Status,
                            tb.Total.Amount as TotalAmount,
                            tb.Paid.Amount as PaidAmount,
                            tb.Balance.Amount as BalanceAmount

                        from 
                            {typeof(TransactionBase).FullName} as tb
                        
                        where
                            (
                                (tb.class = {typeof(Order).FullName} and tb.Paid.Amount > 0) or
                                (tb.class = {typeof(PointOfSale).FullName} and tb.Paid.Amount > 0)
                            )
                        ";

                    var countHql = $@"
                        select 
                            count(tb.Id)

                        from 
                            {typeof(TransactionBase).FullName} as tb
                        
                        where
                            (
                                (tb.class = {typeof(Order).FullName} and tb.Paid.Amount > 0) or
                                (tb.class = {typeof(PointOfSale).FullName} and tb.Paid.Amount > 0)
                            )
                        ";

                    // compose filter
                    var parameters = new Dictionary<string, object>();

                    message.Filter.Compose<Guid>("branchId", value =>
                    {
                        var condition = "td.Branch.Id = :branchId";
                        itemsHql = AppendWithCondition(itemsHql, condition);
                        countHql = AppendWithCondition(countHql, condition);
                        parameters.Add("branchId", value);
                    });

                    message.Filter.Compose<Guid>("customerId", value =>
                    {
                        var condition = "td.Customer.Id = :customerId";
                        itemsHql = AppendWithCondition(itemsHql, condition);
                        countHql = AppendWithCondition(countHql, condition);
                        parameters.Add("customerId", value);
                    });

                    message.Filter.Compose<DateTime>("fromDate", value =>
                    {
                        var condition = "td.PaymentOn >= :fromDate";
                        itemsHql = AppendWithCondition(itemsHql, condition);
                        countHql = AppendWithCondition(countHql, condition);
                        parameters.Add("fromDate", value.StartOfDay());
                    });

                    message.Filter.Compose<DateTime>("toDate", value =>
                    {
                        var condition = "td.PaymentOn <= :toDate";
                        itemsHql = AppendWithCondition(itemsHql, condition);
                        countHql = AppendWithCondition(countHql, condition);
                        parameters.Add("toDate", value.EndOfDay());
                    });

                    // compose order
                    var orders = new List<string>();

                    message.Sorter.Compose("paymentOn", direction =>
                    {
                        itemsHql =  AppendWithOrder(itemsHql, direction, "tb.PaymentOn");
                    });

                    message.Sorter.Compose("branchName", direction =>
                    {
                        itemsHql = AppendWithOrder(itemsHql, direction, "tb.Branch.Name");
                    });

                    message.Sorter.Compose("customerName", direction =>
                    {
                        itemsHql = AppendWithOrder(itemsHql, direction, "tb.Customer.Name");
                    });

                    message.Sorter.Compose("invoice", direction =>
                    {
                        itemsHql = AppendWithOrder(itemsHql, direction, "tb.InvoiceNumber");
                    });

                    message.Sorter.Compose("status", direction =>
                    {
                        itemsHql = AppendWithOrder(itemsHql, direction, "tb.Status");
                    });

                    message.Sorter.Compose("totalAmount", direction =>
                    {
                        itemsHql = AppendWithOrder(itemsHql, direction, "tb.Total.Amount");
                    });

                    message.Sorter.Compose("balanceAmount", direction =>
                    {
                        itemsHql = AppendWithOrder(itemsHql, direction, "tb.Balance.Amount");
                    });

                    var countQuery = session.CreateQuery(countHql);

                    var itemsQuery = session.CreateQuery(itemsHql);

                    foreach (var parameter in parameters)
                    {
                        itemsQuery.SetParameter(parameter.Key, parameter.Value);
                        countQuery.SetParameter(parameter.Key, parameter.Value);
                    }

                    if (message.Pager.IsPaged() != true)
                    {
                        itemsQuery.SetFirstResult(message.Pager.SkipCount);
                        itemsQuery.SetMaxResults(message.Pager.Size);
                    }

                    var countFuture = countQuery
                        .FutureValue<Int32>();

                    var itemsFuture = itemsQuery
                        .SetResultTransformer(Transformers.AliasToBean<Dto.CustomerSalesReportPageItem>())
                        .Future<Dto.CustomerSalesReportPageItem>();

                    response = new Response()
                    {
                        Count = countFuture.Value,
                        Items = itemsFuture.ToList()
                    };

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;

                string AppendWithCondition(string hql, string condition)
                {
                    return hql +
                        Environment.NewLine + new string(' ', 4) + "and" +
                        Environment.NewLine + new string(' ', 4) + condition;
                }

                string AppendWithOrder(string hql, SortDirection direction, string field)
                {
                    if (!hql.Contains("order by"))
                    {
                        hql += hql + Environment.NewLine + "order by";
                    }
                    else
                    {
                        hql += hql + ", ";
                    }

                    return hql + Environment.NewLine + new string(' ', 4) + field + " " + (direction == SortDirection.Ascending ? "asc" : "desc");
                }
            }
        }
    }
    */


    /*
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
                    var query = session
                        .Query<TransactionBase>()
                        .Where(x =>
                            (x is Order && ((Order)x).Paid.Amount > 0) || 
                            (x is PointOfSale && ((PointOfSale)x).Paid.Amount > 0)
                        )
                        .Select(x => new
                        {
                            BranchId = x is Order 
                                ? ((Order)x).Branch.Id : x is PointOfSale 
                                ? ((PointOfSale)x).Branch.Id : Guid.Empty,
                            BranchName = x is Order 
                                ? ((Order)x).Branch.Name : x is PointOfSale 
                                ? ((PointOfSale)x).Branch.Name : string.Empty,
                            CustomerId = x is Order 
                                ? ((Order)x).Customer.Id : x is PointOfSale
                                ? ((PointOfSale)x).Customer.Id : Guid.Empty,
                            CustomerName = x is Order
                                ? ((Order)x).Customer.Name : x is PointOfSale
                                ?   ((PointOfSale)x).Customer.Name : string.Empty,
                            InvoiceNumber = x is Order
                                ? ((Order)x).InvoiceNumber : x is PointOfSale
                                ? ((PointOfSale)x).InvoiceNumber : string.Empty,
                            Status = x is Order
                                ? ((Order)x).Status.ToString() : x is PointOfSale
                                ? ((PointOfSale)x).Status.ToString() : string.Empty,
                            TotalAmount = x is Order
                                ? ((Order)x).Total.Amount : x is PointOfSale
                                ? ((PointOfSale)x).Total.Amount : 0M,
                            PaymentOn = x is Order
                                ? ((Order)x).PaymentOn : x is PointOfSale
                                ? ((PointOfSale)x).PaymentOn : null,
                            PaidAmount = x is Order
                                ? ((Order)x).Paid.Amount : x is PointOfSale
                                ? ((PointOfSale)x).Paid.Amount : 0M,
                            BalanceAmount = x is Order
                                ? ((Order)x).Balance.Amount : x is PointOfSale
                                ? ((PointOfSale)x).Balance.Amount : 0M,
                        });

                    // compose filter
                    message.Filter.Compose<Guid>("branchId", value =>
                    {
                        query = query.Where(x => x.BranchId == value);
                    });

                    message.Filter.Compose<Guid>("customerId", value =>
                    {
                        query = query.Where(x => x.CustomerId == value);
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

                    var countFuture = query
                        .ToFutureValue(x => x.Count());

                    if (message.Pager.IsPaged() != true)
                        message.Pager.RetrieveAll(countFuture.Value);

                    var itemsFuture = query
                        .Select(x => new Dto.CustomerSalesReportPageItem()
                        {
                            PaymentOn = x.PaymentOn,
                            BranchName = x.BranchName,
                            CustomerName = x.CustomerName,
                            InvoiceNumber = x.InvoiceNumber,
                            Status = x.Status,
                            TotalAmount = x.TotalAmount,
                            PaidAmount = x.PaidAmount,
                            BalanceAmount = x.BalanceAmount
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
    */


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
					query = query.Where(x => x.PaymentOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PaymentOn <= value.EndOfDay());
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
						PaymentOn = x.Max(o => o.PaymentOn),
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
						PaymentOn = x.TenderedOn,
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
					query = query.Where(x => x.PaymentOn >= value.StartOfDay());
				});

				message.Filter.Compose<DateTime>("toDate", value =>
				{
					query = query.Where(x => x.PaymentOn <= value.EndOfDay());
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
						PaymentOn = x.Max(o => o.PaymentOn),
						BranchName = x.Key.BranchName,
						CustomerName = x.Key.CustomerName,
						InvoiceNumber = x.Key.InvoiceNumber,
						Status = x.Min(o => o.Order.Status).ToString(),
						TotalAmount = x.Max(o => o.Order.Total.Amount),
						PaidAmount = x.Sum(o => o.Payment.Amount)
					});

				// compose order
				message.Sorter.Compose("paymentOn", direction =>
				{
					groupedQuery = direction == SortDirection.Ascending
						? groupedQuery.OrderBy(x => x.PaymentOn)
						: groupedQuery.OrderByDescending(x => x.PaymentOn);
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
						PaymentOn = x.TenderedOn,
						BranchName = x.Branch.Name,
						CustomerName = x.Customer.Name,
						InvoiceNumber = x.InvoiceNumber,
						Status = "Point of Sale",
						TotalAmount = x.Total.Amount,
						PaidAmount = x.Paid.Amount
					});

				// compose order
				message.Sorter.Compose("paymentOn", direction =>
				{
					selectQuery = direction == SortDirection.Ascending
						? selectQuery.OrderBy(x => x.PaymentOn)
						: selectQuery.OrderByDescending(x => x.PaymentOn);
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
