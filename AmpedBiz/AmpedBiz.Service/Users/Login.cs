using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

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
                    var user = session.Query<User>()
                        .Where(x => 
                            x.Username == message.Username &&
                            x.Password == message.Password
                        )
                        .FirstOrDefault();

                    if (user == null)
                        throw new BusinessException("Invalid username or password!");

                    user.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
