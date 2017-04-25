using AmpedBiz.Core.Entities;
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
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Product>();

                    if (message.ProductIds != null && message.ProductIds.Any())
                    {
                        query = query.Where(x => message.ProductIds.Contains(x.Id));
                    }

                    if (message.SupplierId != Guid.Empty)
                    {
                        query = query.Where(x => x.Supplier.Id == message.SupplierId);
                    }

                    var dtos = query
                        .Select(x => new Dto.ProductInventory()
                        {
                            Id = x.Id,
                            Code = x.Code,
                            Name = x.Name,
                            UnitOfMeasure = x.Inventory.UnitOfMeasure.Name,
                            PackagingUnitOfMeasure = x.Inventory.PackagingUnitOfMeasure.Name,
                            PackagingSize = x.Inventory.PackagingSize,
                            TargetValue = x.Inventory.TargetLevel.Value,
                            AvailableValue = x.Inventory.Available.Value,
                            BadStockValue = x.Inventory.BadStock.Value,
                            BasePriceAmount = x.Inventory.BasePrice.Amount,
                            WholesalePriceAmount = x.Inventory.WholesalePrice.Amount,
                            RetailPriceAmount = x.Inventory.RetailPrice.Amount,
                            BadStockPriceAmount = x.Inventory.BadStockPrice.Amount,
                            DiscountAmount = 0M
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
