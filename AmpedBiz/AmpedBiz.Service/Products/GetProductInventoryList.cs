using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventoryList
    {
        public class Request : IRequest<Response>
        {
            public string SupplierId { get; set; }
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
                    var dtos = session.Query<Product>()
                        .Where(x => x.Supplier.Id == message.SupplierId)
                        .Select(x => new Dto.ProductInventory()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            UnitOfMeasure = x.Inventory.UnitOfMeasure.Name,
                            TargetValue = x.Inventory.TargetLevel.Value,
                            AvailableValue = x.Inventory.Available.Value,
                            BasePriceAmount = x.BasePrice.Amount,
                            RetailPriceAmount = x.RetailPrice.Amount,
                            WholeSalePriceAmount = x.WholeSalePrice.Amount,
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
