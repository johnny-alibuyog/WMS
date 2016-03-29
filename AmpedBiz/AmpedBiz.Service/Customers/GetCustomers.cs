using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
    public class GetCustomers
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.Customer>
        {
            public Response()
            {
            }

            public Response(List<Dto.Customer> items) : base(items)
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
                    var entities = session.Query<Customer>()
                        .Select(x => Mapper.Map<Customer, Dto.Customer>(x, new Dto.Customer()))
                        .ToList();

                    response = new Response(entities);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}