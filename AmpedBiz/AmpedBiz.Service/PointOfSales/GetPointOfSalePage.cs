using AmpedBiz.Core.PointOfSales;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PointOfSales
{
	public class GetPointOfSalePage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.PointOfSalePageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.Query<PointOfSale>();

					// compose filters
					message.Filter.Compose<string>("invoiceNumber", value =>
					{
						query = query.Where(x => x.InvoiceNumber.Contains(value));
					});

					message.Filter.Compose<Guid>("tenderedBy", value =>
					{
						query = query.Where(x => x.TenderedBy.Id == value);
					});

					message.Filter.Compose<Guid>("customer", value =>
					{
						query = query.Where(x => x.Customer.Id == value);
					});

					message.Filter.Compose<PointOfSaleStatus>("status", value =>
					{
						query = query.Where(x => x.Status == value);
					});

					// compose sort
					message.Sorter.Compose("invoiceNumber", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.InvoiceNumber)
							: query.OrderByDescending(x => x.InvoiceNumber);
					});

					message.Sorter.Compose("tenderedOn", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.TenderedOn)
							: query.OrderByDescending(x => x.TenderedOn);
					});

					message.Sorter.Compose("tenderedBy", direction =>
					{
						query = direction == SortDirection.Ascending
							? query
								.OrderBy(x => x.TenderedBy.Person.FirstName)
								.ThenBy(x => x.TenderedBy.Person.LastName)
							: query
								.OrderByDescending(x => x.TenderedBy.Person.FirstName)
								.ThenByDescending(x => x.TenderedBy.Person.LastName);
					});

					message.Sorter.Compose("customer", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Customer.Name)
							: query.OrderByDescending(x => x.Customer.Name);
					});

					message.Sorter.Compose("status", direction =>
					{
						query = direction == SortDirection.Ascending
							? query
								.OrderBy(x => x.Status)
								.ThenByDescending(x => x.TenderedOn)
							: query
								.OrderByDescending(x => x.Status)
								.ThenByDescending(x => x.TenderedOn);
					});

					message.Sorter.Compose("discountAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Discount.Amount)
							: query.OrderByDescending(x => x.Discount.Amount);
					});

					message.Sorter.Compose("subTotalAmount", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.SubTotal.Amount)
							: query.OrderByDescending(x => x.SubTotal.Amount);
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

					var itemsFuture = query
						.Select(x => new Dto.PointOfSalePageItem()
						{
							Id = x.Id,
							InvoiceNumber = x.InvoiceNumber,
							TenderedOn = x.TenderedOn,
							TenderedByName =
								x.CreatedBy.Person.FirstName + " " +
								x.CreatedBy.Person.LastName,
							CustomerName = x.Customer.Name,
							DiscountAmount = x.Discount.Amount,
							SubTotalAmount = x.SubTotal.Amount,
							TotalAmount = x.Total.Amount,
							ReceivedAmount = x.Received.Amount,
							ChangeAmount = x.Change.Amount,
							PaidAmount = x.Paid.Amount,
							BalanceAmount = x.Balance.Amount,
							Status = (Dto.PointOfSaleStatus)x.Status
						})
						.Skip(message.Pager.SkipCount)
						.Take(message.Pager.Size)
						.ToFuture();

					var countFuture = query
						.ToFutureValue(x => x.Count());

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
