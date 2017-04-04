using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class CreateUser
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
                    var entity = new User();
                    entity.Username = message.Username;
                    entity.Password = message.Password;
                    entity.Person = message.Person.MapTo(default(Person));
                    entity.Address = message.Address.MapTo(default(Address));
                    entity.Branch = session.Load<Branch>(message.BranchId);
                    entity.SetRoles(message.Roles
                        .Where(x => x.Assigned)
                        .Select(x => session.Get<Role>(x.Id))
                        .ToList()
                    );
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    message.MapTo(response);

                }

                return response;
            }
        }
    }
}
