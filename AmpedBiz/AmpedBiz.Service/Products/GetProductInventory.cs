﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventory
    {
        public class Request : IRequest<Response>
        {
            public string ProductId { get; set; }
        }

        public class Response : Dto.ProductInventory { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var dto = session.Query<Product>()
                        .Where(x => x.Id == message.ProductId)
                        .Select(x => new Dto.ProductInventory()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            UnitOfMeasure = x.GoodStockInventory.UnitOfMeasure.Name,
                            TargetValue = x.GoodStockInventory.TargetLevel.Value,
                            AvailableValue = x.GoodStockInventory.Available.Value,
                            BasePriceAmount = x.BasePrice.Amount,
                            RetailPriceAmount = x.RetailPrice.Amount,
                            WholeSalePriceAmount = x.WholeSalePrice.Amount,
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
