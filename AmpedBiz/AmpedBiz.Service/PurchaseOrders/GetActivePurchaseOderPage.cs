using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetActivePurchaseOderPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.PurchaseOrderPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var handler = new GetPurchaseOderPage.Handler(this._sessionFactory);
                var request = new GetPurchaseOderPage.Request()
                {
                    Filter = message.Filter,
                    Sorter = message.Sorter,
                    Pager = message.Pager,
                };

                request.Filter["statuses"] = new PurchaseOrderStatus[]
                {
                    PurchaseOrderStatus.New,
                    PurchaseOrderStatus.Submitted,
                    PurchaseOrderStatus.Approved,
                    PurchaseOrderStatus.Paid,
                    PurchaseOrderStatus.Received,
                };

                var result = handler.Handle(request);

                return new Response()
                {
                    Count = result.Count,
                    Items = result.Items
                };
            }
        }
    }
}
