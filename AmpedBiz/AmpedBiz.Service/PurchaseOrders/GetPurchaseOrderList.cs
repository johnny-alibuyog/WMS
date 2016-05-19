using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOrderList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.PurchaseOrder>
        {
            public Response()
            {
            }

            public Response(List<Dto.PurchaseOrder> items) : base(items)
            {
            }
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<PurchaseOrder>()
                        .ToList();

                    var result = Mapper.Map<List<PurchaseOrder>, List<Dto.PurchaseOrder>>(entites);

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}