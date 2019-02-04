using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Inventories;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Transform;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
	public class GetProduct
	{
		public class Request : IRequest<Response>
		{
			public Guid Id { get; set; }
		}

		public class Response : Dto.Product { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
                    var suppliers = session.QueryOver<Supplier>().Cacheable().List();

					var productFuture = session.QueryOver<Product>()
						.Where(x => x.Id == message.Id)
						.Fetch(x => x.Category).Eager
                        .Fetch(x => x.Suppliers).Eager
						.Fetch(x => x.UnitOfMeasures).Eager
						.Fetch(x => x.UnitOfMeasures.First().Prices).Eager
						.TransformUsing(Transformers.DistinctRootEntity)
						.FutureValue();

					var inventoryFuture = session.QueryOver<Inventory>()
						.Where(x =>
							x.Product.Id == message.Id &&
							x.Branch.Id == Context.BranchId
						)
						.FutureValue();

					var product = productFuture.Value;

					var inventory = inventoryFuture.Value;

					product.EnsureExistence($"Product with id {message.Id} does not exists.");

					product.MapTo(response);

                    response.Suppliers = suppliers
                        .Select(x => new Dto.Supplier()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            Assigned = product.Suppliers
                                .Select(o => o.Id)
                                .Contains(x.Id)
                        })
                        .ToList();

					inventory.MapTo(response.Inventory);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}