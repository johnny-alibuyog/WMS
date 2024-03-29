﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.PurchaseOrders;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class ReceivePurchaseOrder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            private void Hydrate(Response response)
            {
                var handler = new GetPurchaseOrder.Handler(this._sessionFactory);
                var hydrated = handler.Handle(new GetPurchaseOrder.Request(response.Id));

                hydrated.MapTo(response);
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    var productIds = message.Receipts
                        .Select(x => x.Product.Id);

                    var products = session.Query<Product>()
                        .Where(x => productIds.Contains(x.Id))
                        .Fetch(x => x.Inventory)
                        .ThenFetch(x => x.UnitOfMeasure)
                        .ToList();

                    entity.State.Process(new PurchaseOrderUpdateReceiptsVisitor()
                    {
                        Receipts = message.Receipts.Select(x => new PurchaseOrderReceipt(
                            batchNumber: x.BatchNumber,
                            receivedBy: session.Load<User>(x.ReceivedBy.Id),
                            receivedOn: x.ReceivedOn ?? DateTime.Now,
                            expiresOn: x.ExpiresOn,
                            product: products.FirstOrDefault(o => o.Id == x.Product.Id),
                            quantity: new Measure(
                                value: x.QuantityValue,
                                unit: products
                                    .Where(o => o.Id == x.Product.Id)
                                    .Select(o => o.Inventory.UnitOfMeasure)
                                    .FirstOrDefault()
                                )
                            )
                        )
                    });

                    session.Save(entity);
                    transaction.Commit();

                    response.Id = entity.Id;
                    //entity.MapTo(response);
                }

                Hydrate(response);

                return response;
            }
        }
    }
}