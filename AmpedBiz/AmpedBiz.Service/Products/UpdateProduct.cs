using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Products
{
    public class UpdateProduct
    {
        public class Request : Dto.Product, IRequest<Response> { }

        public class Response : Dto.Product { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = session.Get<Product>(message.Id);
                    entity.EnsureExistence($"Product with id {message.Id} does not exists.");
                    entity.MapFrom(message);

                    entity.Supplier = session.Load<Supplier>(message.Supplier.Id);
                    entity.Category = session.Load<ProductCategory>(message.Category.Id);

                    entity.Inventory.UnitOfMeasure = session.Load<UnitOfMeasure>(message.Inventory.UnitOfMeasure.Id);
                    entity.Inventory.PackagingUnitOfMeasure = session.Load<UnitOfMeasure>(message.Inventory.PackagingUnitOfMeasure.Id);
                    entity.Inventory.PackagingSize = message.Inventory.PackagingSize ?? 1;
                    entity.Inventory.BasePrice = new Money(message.Inventory.BasePriceAmount ?? 0M, currency);
                    entity.Inventory.WholesalePrice = new Money(message.Inventory.WholesalePriceAmount ?? 0M, currency);
                    entity.Inventory.RetailPrice = new Money(message.Inventory.RetailPriceAmount ?? 0M, currency);
                    entity.Inventory.BadStockPrice = new Money(message.Inventory.BadStockPriceAmount ?? 0M, currency);

                    entity.Inventory.InitialLevel = new Measure(message.Inventory.InitialLevelValue ?? 0M, entity.Inventory.UnitOfMeasure);
                    entity.Inventory.TargetLevel = new Measure(message.Inventory.TargetLevelValue ?? 0M, entity.Inventory.UnitOfMeasure);
                    entity.Inventory.ReorderLevel = new Measure(message.Inventory.ReorderLevelValue ?? 0M, entity.Inventory.UnitOfMeasure);
                    entity.Inventory.MinimumReorderQuantity = new Measure(message.Inventory.MinimumReorderQuantityValue ?? 0M, entity.Inventory.UnitOfMeasure);

                    entity.Inventory.Compute();
                    entity.EnsureValidity();

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}