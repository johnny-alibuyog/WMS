using AmpedBiz.Core.PurchaseOrders;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
	public class GetPurchaseOrderReceivableList
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : List<Dto.PurchaseOrderReceivable>
        {
            public Response() { }

            public Response(IEnumerable<Dto.PurchaseOrderReceivable> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<PurchaseOrder>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Category).Eager
                        .Fetch(x => x.Items.First().Product.Inventories).Eager
                        .Fetch(x => x.Receipts).Eager
                        .Fetch(x => x.Receipts.First().Product).Eager
                        .Fetch(x => x.Receipts.First().Product.Category).Eager
                        .Fetch(x => x.Receipts.First().Product.Inventories).Eager
                        .FutureValue();

                    //var query = session.Query<PurchaseOrder>()
                    //    .Where(x => x.Id == message.Id);

                    //query.FetchMany(x => x.Items)
                    //    .ThenFetch(x => x.Product)
                    //    .ThenFetch(x => x.Inventory)
                    //    .ToFuture();

                    //query.FetchMany(x => x.Receipts)
                    //    .ThenFetch(x => x.Product)
                    //    .ThenFetch(x => x.Inventory)
                    //    .ToFuture();

                    var entity = query.Value;

                    var dtos = Dto.PurchaseOrderReceivable.Evaluate(entity);

                    response = new Response(dtos);

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
