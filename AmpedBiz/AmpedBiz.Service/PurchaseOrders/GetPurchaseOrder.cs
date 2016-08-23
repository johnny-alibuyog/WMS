﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrder
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }

            public Request() { }

            public Request(Guid id)
            {
                this.Id = id;
            }
        }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                if (message.Id == Guid.Empty)
                {
                    var entity = new PurchaseOrder();
                    entity.MapTo(response);

                    return response;
                }

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<PurchaseOrder>()
                        .Where(x => x.Id == message.Id);

                    query
                        .Fetch(x => x.Tax)
                        .Fetch(x => x.ShippingFee)
                        .Fetch(x => x.Shipper)
                        .Fetch(x => x.Supplier)
                        .Fetch(x => x.PaymentType)
                        .Fetch(x => x.Payment)
                        .Fetch(x => x.SubTotal)
                        .Fetch(x => x.Total)
                        .Fetch(x => x.CreatedBy)
                        .Fetch(x => x.SubmittedBy)
                        .Fetch(x => x.ApprovedBy)
                        .Fetch(x => x.PaidBy)
                        .Fetch(x => x.ReceivedBy)
                        .Fetch(x => x.CompletedBy)
                        .Fetch(x => x.CancelledBy)
                        .ToFuture();

                    query
                        .FetchMany(x => x.Items)
                        .ThenFetch(x => x.Product)
                        .ThenFetch(x => x.Inventory)
                        .ToFuture();

                    query
                        .FetchMany(x => x.Payments)
                        .ThenFetch(x => x.PaidBy)
                        .ToFuture();

                    query
                        .FetchMany(x => x.Receipts)
                        .ThenFetch(x => x.Product)
                        .ThenFetch(x => x.Inventory)
                        .ToFuture();

                    var entity = query.ToFutureValue().Value;

                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    entity.MapTo(response);

                    response.Receivables = Dto.PurchaseOrderReceivable.Evaluate(entity);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}