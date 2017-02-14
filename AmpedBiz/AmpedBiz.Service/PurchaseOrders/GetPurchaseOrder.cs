using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
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
                    //var query = session.Query<PurchaseOrder>()
                    //    .Where(x => x.Id == message.Id);

                    //query
                    //    .Fetch(x => x.Tax)
                    //    .Fetch(x => x.ShippingFee)
                    //    .Fetch(x => x.Shipper)
                    //    .Fetch(x => x.Supplier)
                    //    .Fetch(x => x.PaymentType)
                    //    .Fetch(x => x.Payment)
                    //    .Fetch(x => x.SubTotal)
                    //    .Fetch(x => x.Total)
                    //    .Fetch(x => x.CreatedBy)
                    //    .Fetch(x => x.SubmittedBy)
                    //    .Fetch(x => x.ApprovedBy)
                    //    .Fetch(x => x.PaidBy)
                    //    .Fetch(x => x.ReceivedBy)
                    //    .Fetch(x => x.CompletedBy)
                    //    .Fetch(x => x.CancelledBy)
                    //    .ToFuture();

                    //query
                    //    .FetchMany(x => x.Items)
                    //    .ThenFetch(x => x.Product)
                    //    .ThenFetch(x => x.Inventory)
                    //    .ToFuture();

                    //query
                    //    .FetchMany(x => x.Payments)
                    //    .ThenFetch(x => x.PaidBy)
                    //    .ToFuture();

                    //query
                    //    .FetchMany(x => x.Receipts)
                    //    .ThenFetch(x => x.Product)
                    //    .ThenFetch(x => x.Inventory)
                    //    .ToFuture();

                    //var entity = query.ToFutureValue().Value;


                    var entity = session.QueryOver<PurchaseOrder>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Tax).Eager
                        .Fetch(x => x.ShippingFee).Eager
                        .Fetch(x => x.Shipper).Eager
                        .Fetch(x => x.Supplier).Eager
                        .Fetch(x => x.PaymentType).Eager
                        .Fetch(x => x.Payment).Eager
                        .Fetch(x => x.SubTotal).Eager
                        .Fetch(x => x.Total).Eager
                        .Fetch(x => x.CreatedBy).Eager
                        .Fetch(x => x.SubmittedBy).Eager
                        .Fetch(x => x.ApprovedBy).Eager
                        .Fetch(x => x.PaidBy).Eager
                        .Fetch(x => x.ReceivedBy).Eager
                        .Fetch(x => x.CompletedBy).Eager
                        .Fetch(x => x.CancelledBy).Eager
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Supplier).Eager
                        .Fetch(x => x.Items.First().Product.Category).Eager
                        .Fetch(x => x.Items.First().Product.Inventory).Eager
                        .Fetch(x => x.Payments).Eager
                        .Fetch(x => x.Payments.First().PaidBy).Eager
                        .Fetch(x => x.Receipts).Eager
                        .Fetch(x => x.Receipts.First().Product).Eager
                        .Fetch(x => x.Receipts.First().Product.Supplier).Eager
                        .Fetch(x => x.Receipts.First().Product.Category).Eager
                        .Fetch(x => x.Receipts.First().Product.Inventory).Eager
                        .SingleOrDefault();

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