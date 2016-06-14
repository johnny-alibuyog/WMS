using MediatR;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Common.Extentions;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderStatusLookup
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<KeyValuePair<int, string>>
        {
            public Response() { }

            public Response(List<KeyValuePair<int, string>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                return new Response(EnumExtention.ToLookup<Dto.PurchaseOrderStatus>());
            }
        }
    }
}
