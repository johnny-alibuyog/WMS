using System.Linq;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Users
{
    public class GetInitialUser
    {
        public class Request : IRequest<Response> { }

        public class Response : Dto.User { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var roles = session.Query<Entity.Role>();

                    response.Roles = roles
                        .Select(x => new Dto.Role()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Assigned = false,
                        })
                        .ToList();

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
