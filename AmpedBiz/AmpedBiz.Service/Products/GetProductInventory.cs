﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System;

namespace AmpedBiz.Service.Products
{
    public class GetProductInventory
    {
        public class Request : IRequest<Response>
        {
            public Guid ProductId { get; set; }
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
                            UnitOfMeasure = x.Inventory.UnitOfMeasure.Name,
                            TargetValue = x.Inventory.TargetLevel.Value,
                            AvailableValue = x.Inventory.Available.Value,
                            BadStockValue = x.Inventory.BadStock.Value,
                            BasePriceAmount = x.Inventory.BasePrice.Amount,
                            RetailPriceAmount = x.Inventory.RetailPrice.Amount,
                            WholeSalePriceAmount = x.Inventory.WholesalePrice.Amount,
                            BadStockPriceAmount = x.Inventory.BadStockPrice.Amount,
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
