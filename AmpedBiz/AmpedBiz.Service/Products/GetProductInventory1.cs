using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Products;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Products
{
    class GetProductInventory1
    {
        public class Request : IRequest<Response>
        {
            public Guid ProductId { get; set; }
        }

        public class Response : Dto.ProductInventory1 { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var product = session.QueryOver<Product>()
                        .Where(x => x.Id == message.ProductId)
                        .Fetch(x => x.UnitOfMeasures).Eager
                        .Fetch(x => x.UnitOfMeasures.First().Prices).Eager
                        .SingleOrDefault();

                    response = new Response()
                    {
                        Id = product.Id,
                        Code = product.Code,
                        Name = product.Name,
                        UnitOfMeasure = product.UnitOfMeasures
                            .Select(x => new Dto.ProductInventoryUnitOfMeasure()
                            {
                                UnitOfMeasure = new Lookup<string>()
                                {
                                    Id = x.UnitOfMeasure.Id,
                                    Name = x.UnitOfMeasure.Name
                                },
                                AvailableValue = product.ConvertValue(
                                    measure: product.Inventory.Available, 
                                    toUnit: x.UnitOfMeasure
                                )
                            })
                            .ToList()
                    };

                    //var dto = session.Query<Product>()
                    //    .Where(x => x.Id == message.ProductId)
                    //    .Select(x => new Dto.ProductInventory()
                    //    {
                    //        Id = x.Id,
                    //        Code = x.Code,
                    //        Name = x.Name,
                    //        UnitOfMeasure = x.Inventory.UnitOfMeasure.Name,
                    //        PackagingUnitOfMeasure = x.Inventory.PackagingUnitOfMeasure.Name,
                    //        PackagingSize = x.Inventory.PackagingSize,
                    //        TargetValue = x.Inventory.TargetLevel.Value,
                    //        AvailableValue = x.Inventory.Available.Value,
                    //        BadStockValue = x.Inventory.BadStock.Value,
                    //        BasePriceAmount = x.Inventory.BasePrice.Amount,
                    //        WholesalePriceAmount = x.Inventory.WholesalePrice.Amount,
                    //        RetailPriceAmount = x.Inventory.RetailPrice.Amount,
                    //        BadStockPriceAmount = x.Inventory.BadStockPrice.Amount,
                    //        DiscountAmount = 0M
                    //    })
                    //    .FirstOrDefault();

                    //dto.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
