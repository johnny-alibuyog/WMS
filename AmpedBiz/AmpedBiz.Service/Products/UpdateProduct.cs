﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;
using static AmpedBiz.Common.Extentions.TypeExtentions;

namespace AmpedBiz.Service.Products
{
    public class UpdateProduct
    {
        public class Request : Dto.Product, IRequest<Response> { }

        public class Response : Dto.Product { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = session.Query<Product>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Inventory)
                        .FetchMany(x => x.UnitOfMeasures)
                        .ThenFetchMany(x => x.Prices)
                        .ToFutureValue().Value;

                    entity.EnsureExistence($"Product with id {message.Id} does not exists.");

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
                        UnitOfMeasures = message.UnitOfMeasures
                            .Select(x => new ProductUnitOfMeasure(
                                id: x.Id,
                                size: x.Size,
                                barcode: x.Barcode,
                                isDefault: x.IsDefault ?? false,
                                isStandard: x.IsStandard ?? false,
                                unitOfMeasure: session.Load<UnitOfMeasure>(x.UnitOfMeasure.Id),
                                standardEquivalentValue: x.StandardEquivalentValue ?? 0M,
                                prices: x.Prices
                                    .Select(o => new ProductUnitOfMeasurePrice(
                                        id: o.Id,
                                        pricing: session.Load<Pricing>(o.Pricing.Id),
                                        price: new Money(amount: o.PriceAmount ?? 0M, currency: currency)
                                    ))
                                    .ToList()
                            ))
                            .ToList()
                    });

                    entity.EnsureValidity();

                    var standard = entity.UnitOfMeasures.FirstOrDefault(o => o.IsStandard);

                    entity.Inventory.Accept(new InventoryUpdateVisitor()
                    {
                        InitialLevel = new Measure(message.Inventory.InitialLevelValue ?? 0M, standard.UnitOfMeasure),
                        TargetLevel = new Measure(message.Inventory.TargetLevelValue ?? 0M, standard.UnitOfMeasure),
                        ReorderLevel = new Measure(message.Inventory.ReorderLevelValue ?? 0M, standard.UnitOfMeasure),
                        MinimumReorderQuantity = new Measure(message.Inventory.MinimumReorderQuantityValue ?? 0M, standard.UnitOfMeasure)
                    });

                    entity.Inventory.EnsureValidity();

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}