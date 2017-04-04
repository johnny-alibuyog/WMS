using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Users
{
    public class Login
    {
        public class Request : Dto.User, IRequest<Response> { }

        public class Response : Dto.User { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var user = session.QueryOver<User>()
                        .Where(x =>
                            x.Username == message.Username &&
                            x.Password == message.Password
                        )
                        .Fetch(x => x.Branch).Eager
                        .Fetch(x => x.Roles).Eager
                        .SingleOrDefault();

                    user.EnsureExistence("Invalid username or password!");
                    user.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
