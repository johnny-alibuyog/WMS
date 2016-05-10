using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.Product>
        {
            public Response()
            {
            }

            public Response(List<Dto.Product> items) : base(items)
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
                    var entites = session.Query<Product>()
                        .ToList();

                    var result = Mapper.Map<List<Product>, List<Dto.Product>>(entites);

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}