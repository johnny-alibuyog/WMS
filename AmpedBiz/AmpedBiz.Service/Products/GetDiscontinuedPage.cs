﻿using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetDiscontinuedPage
	{
		public class Request : PageRequest, IRequest<Response> { }

		public class Response : PageResponse<Dto.DiscontinuedPageItem> { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var query = session.Query<Product>()
						.Where(x => x.Discontinued);

					// compose filters
					message.Filter.Compose<string>("name", value =>
					{
						query = query.Where(x => x.Name.Contains(value.ToLower()));
					});

					// compose sort
					message.Sorter.Compose("code", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Code)
							: query.OrderByDescending(x => x.Code);
					});

					message.Sorter.Compose("name", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Name)
							: query.OrderByDescending(x => x.Name);
					});

					message.Sorter.Compose("description", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Description)
							: query.OrderByDescending(x => x.Description);
					});

					message.Sorter.Compose("category", direction =>
					{
						query = direction == SortDirection.Ascending
							? query.OrderBy(x => x.Category.Name)
							: query.OrderByDescending(x => x.Category.Name);
					});

					// TODO: Refactor

					//message.Sorter.Compose("basePrice", direction =>
					//{
					//    query = direction == SortDirection.Ascending
					//        ? query.OrderBy(x => x.Inventory.BasePrice.Amount)
					//        : query.OrderByDescending(x => x.Inventory.BasePrice.Amount);
					//});

					//message.Sorter.Compose("wholesalePrice", direction =>
					//{
					//    query = direction == SortDirection.Ascending
					//        ? query.OrderBy(x => x.Inventory.WholesalePrice.Amount)
					//        : query.OrderByDescending(x => x.Inventory.WholesalePrice.Amount);
					//});

					//message.Sorter.Compose("retailPrice", direction =>
					//{
					//    query = direction == SortDirection.Ascending
					//        ? query.OrderBy(x => x.Inventory.RetailPrice.Amount)
					//        : query.OrderByDescending(x => x.Inventory.RetailPrice.Amount);
					//});

					var itemsFuture = query
						.Select(x => new Dto.DiscontinuedPageItem()
						{
							Id = x.Id.ToString(),
							Code = x.Code,
							Name = x.Name,
							Description = x.Description,
							CategoryName = x.Category.Name,
							Image = x.Image,
							//BasePriceAmount = x.Inventory.BasePrice.Amount,
							//WholesalePriceAmount = x.Inventory.WholesalePrice.Amount,
							//RetailPriceAmount = x.Inventory.RetailPrice.Amount,
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
