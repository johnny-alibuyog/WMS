using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System.Collections.Generic;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderStatusLookup
    {
        public class Request : IRequest<Response> { }

        public class Response : List<Lookup<Dto.PurchaseOrderStatus>>
        {
            public Response() { }

            public Response(List<Lookup<Dto.PurchaseOrderStatus>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                return new Response(EnumExtention.ToLookup<Dto.PurchaseOrderStatus>());
            }
        }
    }
}
