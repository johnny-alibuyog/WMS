using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }
        }

        public class Response : List<Dto.PurchaseOrder>
        {
            public Response() { }

            public Response(List<Dto.PurchaseOrder> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.QueryOver<PurchaseOrder>()
                        .Fetch(x => x.Tax).Eager
                        .Fetch(x => x.ShippingFee).Eager
                        .Fetch(x => x.Shipper).Eager
                        .Fetch(x => x.Supplier).Eager
                        .Fetch(x => x.PaymentType).Eager
                        .Fetch(x => x.Paid).Eager
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
                        .Fetch(x => x.Items.First().Product.Inventory).Eager
                        .Fetch(x => x.Payments).Eager
                        .Fetch(x => x.Payments.First().PaidBy).Eager
                        .Fetch(x => x.Receipts).Eager
                        .Fetch(x => x.Receipts.First().Product).Eager
                        .Fetch(x => x.Receipts.First().Product.Inventory).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .List();

                    var dtos = entites.MapTo(default(List<Dto.PurchaseOrder>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}