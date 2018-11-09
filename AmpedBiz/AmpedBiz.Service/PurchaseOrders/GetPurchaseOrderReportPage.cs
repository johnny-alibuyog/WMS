using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
	public class GetPurchaseOrderReportPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.PurchaseOrderReportPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.Query<PurchaseOrder>();

					// compose filter
					message.Filter.Compose<Guid>("branchId", value =>
					{
						query = query.Where(x => x.Branch.Id == value);
					});

					message.Filter.Compose<Guid>("supplierId", value =>
					{
						query = query.Where(x => x.Supplier.Id == value);
					});

					message.Filter.Compose<PurchaseOrderStatus>("status", value =>
					{
						query = query.Where(x => x.Status == value);
					});

					message.Filter.Compose<DateTime>("fromDate", value =>
					{
						query = query.Where(x => x.CreatedOn >= value);
					});

					message.Filter.Compose<DateTime>("toDate", value =>
					{
						query = query.Where(x => x.CreatedOn <= value);
					});

					// compose order
					message.Sorter.Compose("createdOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.CreatedOn)
							: query.OrderByDescending(x => x.CreatedOn);
					});

					message.Sorter.Compose("branchName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Branch.Name)
							: query.OrderByDescending(x => x.Branch.Name);
					});

					message.Sorter.Compose("supplierName", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Supplier.Name)
							: query.OrderByDescending(x => x.Supplier.Name);
					});

					message.Sorter.Compose("voucherNumber", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.VoucherNumber)
							: query.OrderByDescending(x => x.VoucherNumber);
					});

					message.Sorter.Compose("createdBy", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x =>
								x.CreatedBy.Person.FirstName + " " +
								x.CreatedBy.Person.LastName
							)
							: query.OrderByDescending(x =>
								x.CreatedBy.Person.FirstName + " " +
								x.CreatedBy.Person.LastName
							);
					});

					message.Sorter.Compose("approvedBy", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x =>
								x.ApprovedBy.Person.FirstName + " " +
								x.ApprovedBy.Person.LastName
							)
							: query.OrderByDescending(x =>
								x.ApprovedBy.Person.FirstName + " " +
								x.ApprovedBy.Person.LastName
							);
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
							? query.OrderBy(x => x.Total.Amount)
							: query.OrderByDescending(x => x.Total.Amount);
					});

					message.Sorter.Compose("paidAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Paid.Amount)
							: query.OrderByDescending(x => x.Paid.Amount);
					});

					var countFuture = query
						.ToFutureValue(x => x.Count());

					if (message.Pager.IsPaged() != true)
						message.Pager.RetrieveAll(countFuture.Value);

					var itemsFuture = query
						.Select(x => new Dto.PurchaseOrderReportPageItem()
						{
							Id = x.Id,
							CreatedOn = x.CreatedOn,
							BranchName = x.Branch.Name,
							SupplierName = x.Supplier.Name,
							VoucherNumber = x.VoucherNumber,
							CreatedByName =
								x.CreatedBy.Person.FirstName + " " +
								x.CreatedBy.Person.LastName,
							ApprovedByName =
								x.ApprovedBy.Person.FirstName + " " +
								x.ApprovedBy.Person.LastName,
							Status = x.Status.As<Dto.PurchaseOrderStatus>(),
							TotalAmount = x.Total != null ? x.Total.Amount : 0M,
							PaidAmount = x.Paid != null ? x.Paid.Amount : 0M
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
