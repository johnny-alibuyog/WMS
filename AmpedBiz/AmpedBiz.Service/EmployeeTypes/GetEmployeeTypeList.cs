using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.EmployeeTypes
{
    public class GetEmployeeTypeList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.EmployeeType>
        {
            public Response() { }

            public Response(List<Dto.EmployeeType> items) : base(items) { }
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
                    var entites = session.Query<EmployeeType>()
                        .Select(x => new Dto.EmployeeType()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToList();

                    response = new Response(entites);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
