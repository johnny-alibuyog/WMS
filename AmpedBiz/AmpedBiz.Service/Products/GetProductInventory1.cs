using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using AmpedBiz.Data.Context;
using ExpressMapper.Extensions;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventory1
    {
        public class Request : IRequest<Response>
        {
            public Guid ProductId { get; set; }
        }

        public class Response : Dto.ProductInventory1 { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var inventory = session.QueryOver<Inventory>()
                        .Where(x => x.Product.Id == message.ProductId)
                        .Fetch(x => x.Product).Eager
                        .Fetch(x => x.Product.UnitOfMeasures).Eager
                        .Fetch(x => x.Product.UnitOfMeasures.First().Prices).Eager
                        .SingleOrDefault();

                    response = new Response()
                    {
                        Id = inventory.Product.Id,
                        Code = inventory.Product.Code,
                        Name = inventory.Product.Name,
                        UnitOfMeasures = inventory.Product.UnitOfMeasures
                            .Select(x => new Dto.ProductInventoryUnitOfMeasure()
                            {
                                IsDefault = x.IsDefault,
                                IsStandard = x.IsStandard,
                                UnitOfMeasure = x.UnitOfMeasure
                                    .MapTo(default(Dto.UnitOfMeasure)),
                                Available = inventory
                                    .Convert(o => o.Available)
                                    .To(x.UnitOfMeasure)
                                    .MapTo(default(Dto.Measure)),
                                Standard = inventory.Product
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
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
