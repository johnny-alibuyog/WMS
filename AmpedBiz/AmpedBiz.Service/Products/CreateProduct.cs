using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class CreateProduct
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
                    var exists = session.Query<Product>().Any(x => x.Id == message.Id);
                    exists.Assert($"Product with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = new Product(message.Id);

                    entity.Accept(new ProductUpdateVisitor()
                    {
                        Code = message.Code,
                        Name = message.Name,
                        Description = message.Description,
                        Supplier = (!message.Supplier?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Supplier>(message.Supplier.Id) : null,
                        Category = (!message.Category?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<ProductCategory>(message.Category.Id) : null,
                        Image = message.Image,
                        Discontinued = message.Discontinued,
                    });

                    entity.Inventory.Accept(new InventoryUpdateVisitor()
                    {
                        UnitOfMeasure = session.Load<UnitOfMeasure>(message.Inventory.UnitOfMeasure.Id),
                        PackagingUnitOfMeasure = session.Load<UnitOfMeasure>(message.Inventory.PackagingUnitOfMeasure.Id),
                        PackagingSize = message.Inventory.PackagingSize ?? 1,
                        BasePrice = new Money(message.Inventory.BasePriceAmount ?? 0M, currency),
                        WholesalePrice = new Money(message.Inventory.WholesalePriceAmount ?? 0M, currency),
                        RetailPrice = new Money(message.Inventory.RetailPriceAmount ?? 0M, currency),
                        BadStockPrice = new Money(message.Inventory.BadStockPriceAmount ?? 0M, currency),
                        InitialLevel = new Measure(message.Inventory.InitialLevelValue ?? 0M, entity.Inventory.UnitOfMeasure),
                        TargetLevel = new Measure(message.Inventory.TargetLevelValue ?? 0M, entity.Inventory.UnitOfMeasure),
                        ReorderLevel = new Measure(message.Inventory.ReorderLevelValue ?? 0M, entity.Inventory.UnitOfMeasure),
                        MinimumReorderQuantity = new Measure(message.Inventory.MinimumReorderQuantityValue ?? 0M, entity.Inventory.UnitOfMeasure),
                    });

                    entity.EnsureValidity();

                    session.Save(entity);

                    transaction.Commit();

                    entity.MapTo(response);;
                }

                return response;
            }
        }
    }
}
