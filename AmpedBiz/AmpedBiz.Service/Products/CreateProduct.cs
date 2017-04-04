﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using NHibernate.Linq;
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
                    var entity = message.MapTo(new Product(message.Id));

                    entity.Supplier = session.Load<Supplier>(message.Supplier.Id);
                    entity.Category = session.Load<ProductCategory>(message.Category.Id);

                    entity.Inventory.UnitOfMeasure = session.Load<UnitOfMeasure>(message.Inventory.UnitOfMeasure.Id);
                    entity.Inventory.PackagingUnitOfMeasure = session.Load<UnitOfMeasure>(message.Inventory.PackagingUnitOfMeasure.Id);
                    entity.Inventory.PackagingSize = message.Inventory.PackagingSize ?? 1;
                    entity.Inventory.BasePrice = new Money(message.Inventory.BasePriceAmount ?? 0M, currency);
                    entity.Inventory.DistributorPrice = new Money(message.Inventory.DistributorPriceAmount ?? 0M, currency);
                    entity.Inventory.ListPrice = new Money(message.Inventory.ListPriceAmount ?? 0M, currency);
                    entity.Inventory.BadStockPrice = new Money(message.Inventory.BadStockPriceAmount ?? 0M, currency);

                    entity.Inventory.InitialLevel = new Measure(message.Inventory.InitialLevelValue ?? 0M, entity.Inventory.UnitOfMeasure);
                    entity.Inventory.TargetLevel = new Measure(message.Inventory.TargetLevelValue ?? 0M, entity.Inventory.UnitOfMeasure);
                    entity.Inventory.ReorderLevel = new Measure(message.Inventory.ReorderLevelValue ?? 0M, entity.Inventory.UnitOfMeasure);
                    entity.Inventory.MinimumReorderQuantity = new Measure(message.Inventory.MinimumReorderQuantityValue ?? 0M, entity.Inventory.UnitOfMeasure);

                    entity.Inventory.Compute();
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
