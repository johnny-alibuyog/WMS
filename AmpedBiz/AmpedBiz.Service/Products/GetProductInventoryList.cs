using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventoryList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] ProductIds { get; set; }

            public Guid SupplierId { get; set; }
        }

        public class Response : List<Dto.ProductInventory>
        {
            public Response() { }

            public Response(List<Dto.ProductInventory> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Inventory>();

                    if (!message.ProductIds.IsNullOrEmpty())
                    {
                        query = query.Where(x => message.ProductIds.Contains(x.Product.Id));
                    }

                    if (!message.SupplierId.IsNullOrDefault())
                    {
                        query = query.Where(x => x.Product.Supplier.Id == message.SupplierId);
                    }

                    var productInventories = query
                        .Fetch(x => x.Product)
                        .ThenFetchMany(x => x.UnitOfMeasures)
                        .ThenFetchMany(x => x.Prices)
                        .ToList();

                    var dtos = productInventories
                        .Select(productInventory => new Dto.ProductInventory()
                        {
                            Id = productInventory.Product.Id,
                            Code = productInventory.Product.Code,
                            Name = productInventory.Product.Name,
                            UnitOfMeasures = productInventory.Product.UnitOfMeasures
                                .Select(x => new Dto.ProductInventoryUnitOfMeasure()
                                {
                                    IsDefault = x.IsDefault,
                                    IsStandard = x.IsStandard,
                                    UnitOfMeasure = x.UnitOfMeasure
                                        .MapTo(default(Dto.UnitOfMeasure)),
                                    Available = productInventory
                                        .Convert(o => o.Available)
                                        .To(x.UnitOfMeasure)
                                        .MapTo(default(Dto.Measure)),
                                    TargetLevel = productInventory
                                        .Convert(o => o.TargetLevel)
                                        .To(x.UnitOfMeasure)
                                        .MapTo(default(Dto.Measure)),
                                    BadStock = productInventory
                                        .Convert(o => o.BadStock)
                                        .To(x.UnitOfMeasure)
                                        .MapTo(default(Dto.Measure)),
                                    Standard = x.Product
                                        .StandardEquivalentMeasureOf(x.UnitOfMeasure)
                                        .MapTo(default(Dto.Measure)),
                                    Prices = x.Prices
                                        .Select(o => new Dto.ProductInventoryUnitOfMeasurePrice()
                                        {
                                            Pricing = new Lookup<string>()
                                            {
                                                Id = o.Pricing.Id,
                                                Name = o.Pricing.Name
                                            },
                                            PriceAmount = o.Price.Amount
                                        })
                                        .ToList()
                                })
                                .ToList()
                        })
                        .ToList();

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
