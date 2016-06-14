using AmpedBiz.Core.Entities;
using AmpedBiz.Service.Common;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetSubmittedPurchaseOrdersPage
    {
        public class Request : PageRequest, IRequest<Response> { }

        public class Response : PageResponse<Dto.PurchaseOrderPageItem> { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var handler = new GetPurchaseOrderPage.Handler(this._sessionFactory);
                var result = handler.Handle(new GetPurchaseOrderPage.Request()
                {
                    Filter = new Filter() { { "status", PurchaseOrderStatus.Submitted } },
                    Sorter = message.Sorter,
                    Pager = message.Pager,
                });

                return new Response()
                {
                    Count = result.Count,
                    Items = result.Items
                };
            }
        }
    }
}
