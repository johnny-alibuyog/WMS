using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Users
{
    public class Override
    {
        public class Request : Dto.User, IRequest<Response> { }

        public class Response : Dto.User { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
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
                    user.IsManager().Assert("You do not have overriding rights.");
                    user.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
