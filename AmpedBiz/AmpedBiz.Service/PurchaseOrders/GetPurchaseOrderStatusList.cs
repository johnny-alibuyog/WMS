using AmpedBiz.Service.Dto;
using MediatR;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderStatusList
    {
        public class Request : IRequest<Response> { }

        public class Response : List<Dto.PurchaseOrderStatus>
        {
            public Response() { }

            public Response(IEnumerable<Dto.PurchaseOrderStatus> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                return new Response(Enum.GetValues(typeof(PurchaseOrderStatus)).Cast<PurchaseOrderStatus>());
            }
        }
    }
}
