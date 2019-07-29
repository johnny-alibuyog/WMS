using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Products;
using AmpedBiz.Core.Returns;
using AmpedBiz.Core.Returns.Services;
using AmpedBiz.Core.Users;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Returns
{
	public class CreateReturn
	{
		public class Request : Dto.Return, IRequest<Response> { }

		public class Response : Dto.Return { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var productIds = message.Items.Select(x => x.Product.Id);

					var currency = session.Load<Currency>(Currency.PHP.Id);

					var products = session.Query<Product>()
						.Where(x => productIds.Contains(x.Id))
						.Fetch(x => x.Inventories)
						.ToList()
                        .ToDictionary(x => x.Id);

					var entity = new Return();

                    entity.Accept(new ReturnSaveVisitor()
                    {
                        Branch = session.Load<Branch>(message.Branch.Id),
                        Pricing = session.Load<Pricing>(message.Pricing.Id),
						Customer = session.Load<Customer>(message.Customer.Id),
						ReturnedBy = session.Load<User>(message.ReturnedBy.Id),
						ReturnedOn = message.ReturnedOn ?? new DateTime(),
						Remarks = message.Remarks,
						Items = message.Items
							.Select((x, i) => new ReturnItem(
                                sequence: i,
								product: products[x.Product.Id],
								reason: session.Load<ReturnReason>(x.Reason.Id),
								quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
								standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id)),
								unitPrice: new Money(x.UnitPriceAmount, currency)
							))
							.ToList()
					});
					entity.EnsureValidity();

					session.Save(entity);

					transaction.Commit();

					entity.MapTo(response);

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
