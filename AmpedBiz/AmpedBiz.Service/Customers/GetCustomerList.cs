using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
    public class GetCustomerList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.Customer>
        {
            public Response() { }

            public Response(List<Dto.Customer> items) : base(items) { }
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
                    var entites = session.Query<Customer>()
                        .ToList();

                    var result = Mapper.Map<List<Customer>, List<Dto.Customer>>(entites);

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}