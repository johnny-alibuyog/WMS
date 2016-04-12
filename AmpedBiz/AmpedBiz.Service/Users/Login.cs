using System.Linq;
using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Users
{
    public class Login
    {
        public class Request : Dto.User, IRequest<Response> { }

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
                    var user = session.Query<Entity.User>()
                        .Where(x => 
                            x.Username == message.Username &&
                            x.Password == message.Password
                        )
                        .FirstOrDefault();

                    if (user == null)
                        throw new BusinessException("Invalid username or password!");

                    Mapper.Map<Entity.User, Dto.User>(user, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
