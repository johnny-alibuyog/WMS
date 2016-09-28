using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Returns;
using MediatR;
using NHibernate;
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
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var productIds = message.Items.Select(x => x.Product.Id);

                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    var products = session.Query<Product>()
                        .Where(x => productIds.Contains(x.Id))
                        .Fetch(x => x.Inventory)
                        .ToList();

                    Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<Guid, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    var entity = new Return();

                    entity.Accept(new ReturnCreateVisitor()
                    {
                        Branch = session.Load<Branch>(message.Branch.Id),
                        Customer = session.Load<Customer>(message.Customer.Id),
                        ReturnedBy = session.Load<User>(message.ReturnedBy.Id),
                        ReturnedOn = message.ReturnedOn ?? new DateTime(),
                        Remarks = message.Remarks,
                        Items = message.Items
                            .Select(x => new ReturnItem(
                                product: GetProduct(x.Product.Id),
                                returnReason: session.Load<ReturnReason>(x.ReturnReason.Id),
                                quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id)),
                                unitPrice: new Money(x.UnitPriceAmount, currency)
                            ))
                            .ToList()
                    });

                    session.Save(entity);

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
