using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductUOMInventoryList
    {
        public class Request : IRequest<Response>
        {
            public Guid ProductId { get; set; }
        }

        public class Response : Dto.ProductInventory { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                // TODO: refactor

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var dto = session.Query<Product>()
                        .Where(x => x.Id == message.ProductId)
                        .Select(x => new Dto.ProductInventory()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            //UnitOfMeasure = x.Inventory.UnitOfMeasure.Name,
                            //PackagingUnitOfMeasure = x.Inventory.PackagingUnitOfMeasure.Name,
                            //PackagingSize = x.Inventory.PackagingSize,
                            TargetValue = x.Inventory.TargetLevel.Value,
                            AvailableValue = x.Inventory.Available.Value,
                            BadStockValue = x.Inventory.BadStock.Value,
                            //BasePriceAmount = x.Inventory.BasePrice.Amount,
                            //WholesalePriceAmount = x.Inventory.WholesalePrice.Amount,
                            //RetailPriceAmount = x.Inventory.RetailPrice.Amount,
                            //BadStockPriceAmount = x.Inventory.BadStockPrice.Amount,
                            DiscountAmount = 0M
                        })
                        .FirstOrDefault();

                    dto.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
