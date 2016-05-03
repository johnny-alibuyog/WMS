using System.Linq;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Users
{
    public class CreateUser
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
                    var entity = new Entity.User(message.Id);
                    entity.Username = message.Username;
                    entity.Password = message.Password;
                    entity.Person = Mapper.Map<Dto.Person, Entity.Person>(message.Person);
                    entity.Address = Mapper.Map<Dto.Address, Entity.Address>(message.Address);
                    entity.Branch = session.Load<Entity.Branch>(message.BranchId);
                    entity.SetRoles(message.Roles
                        .Where(x => x.Assigned)
                        .Select(x => session.Get<Entity.Role>(x.Id))
                        .ToList()
                    );

                    session.Save(entity);

                    Mapper.Map<Dto.User, Dto.User>(message, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
