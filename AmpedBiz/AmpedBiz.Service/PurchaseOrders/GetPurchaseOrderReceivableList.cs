using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
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
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<PurchaseOrder>()
                        .Where(x => x.Id == message.Id);

                    query.FetchMany(x => x.Items)
                        .ThenFetch(x => x.Product)
                        .ThenFetch(x => x.GoodStockInventory)
                        .ToFuture();

                    query.FetchMany(x => x.Receipts)
                        .ThenFetch(x => x.Product)
                        .ThenFetch(x => x.GoodStockInventory)
                        .ToFuture();

                    var entity = query.ToFutureValue().Value;

                    var dtos = Dto.PurchaseOrderReceivable.Evaluate(entity);

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;

            }
        }
    }
}
