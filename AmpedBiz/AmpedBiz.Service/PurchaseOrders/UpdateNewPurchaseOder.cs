using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.PurchaseOrders;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;


namespace AmpedBiz.Service.PurchaseOrders
{
    public class UpdateNewPurchaseOder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            private void Hydrate(Response response)
            {
                var handler = new GetPurchaseOrder.Handler(this._sessionFactory);
                var hydrated = handler.Handle(new GetPurchaseOrder.Request(response.Id));

                hydrated.MapTo(response);
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PurchaseOrder>(message.Id);

                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant

                    var productIds = message.Items.Select(x => x.Product.Id);

                    var products = session.Query<Product>()
                        .Where(x => productIds.Contains(x.Id))
                        .Fetch(x => x.Inventory)
                        .ToList();

                    Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<Guid, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    entity.State.Process(new PurchaseOrderSaveVisitor()
                    {
                        CreatedBy = (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        CreatedOn = message?.CreatedOn ?? DateTime.Now,
                        ExpectedOn = message?.ExpectedOn,
                        PaymentType = (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        Supplier = (!message?.Supplier?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Supplier>(message.Supplier.Id) : null,
                        Shipper = null,
                        ShippingFee = new Money(message.ShippingFeeAmount, currency),
                        Tax = new Money(message.TaxAmount, currency),
                        Items = message.Items
                            .Select(x => new PurchaseOrderItem(
                                id: x.Id,
                                product: GetProduct(x.Product.Id),
                                unitCost: new Money(x.UnitCostAmount, currency),
                                quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id))
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
