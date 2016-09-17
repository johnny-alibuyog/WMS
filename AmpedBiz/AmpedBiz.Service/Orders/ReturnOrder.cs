using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Orders
{
    public class ReturnOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            private void Hydrate(Response response)
            {
                var handler = new GetOrder.Handler(this._sessionFactory);
                var hydrated = handler.Handle(new GetOrder.Request(response.Id));

                hydrated.MapTo(response);
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var productIds = message.Items.Select(x => x.Product.Id);

                    var entity = session.QueryOver<Order>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Returns).Eager
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Inventory).Eager
                        .SingleOrDefault();

                    var products = entity.Items
                        .Select(x => x.Product)
                        .ToList();

                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    Func<string, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<string, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    entity.State.Process(new OrderReturnVisitor()
                    {
                        Returns = message.Returns.Select(x => new OrderReturn(
                            product: GetProduct(x.Product.Id),
                            returnedOn: x.ReturnedOn ?? DateTime.Now,
                            returnedBy: session.Load<User>(x.ReturnedBy.Id),
                            quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id)),
                            discountRate: x.DiscountRate,
                            discount: new Money(x.DiscountAmount, currency),
                            unitPrice: new Money(x.UnitPriceAmount, currency)
                        ))
                    });

                    session.Save(entity);
                    transaction.Commit();

                    response.Id = entity.Id;
                    //entity.MapTo(response);
                }

                Hydrate(response);

                return response;
            }
        }
    }
}
