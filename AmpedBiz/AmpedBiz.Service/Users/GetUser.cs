using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUser
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

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
                    var entity = session.QueryOver<User>()
                        .Where(x => x.Id == message.Id)
                        .Left.JoinQueryOver(x => x.UserRoles)
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .SingleOrDefault();

                    //var entity = query.Value;
                    var roles = session.Query<Role>().ToList();

                    response.Id = entity.Id;
                    response.Username = entity.Username;
                    response.Password = entity.Password;
                    response.Person = entity.Person.MapTo(default(Dto.Person));
                    response.Address = entity.Address.MapTo(default(Dto.Address));
                    response.BranchId = entity.Branch.Id;
                    response.Roles = roles
                        .Select(x => new Dto.Role()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Assigned = entity.UserRoles
                                .Select(y => y.Role.Id)
                                .Contains(x.Id)
                        })
                        .ToList();

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}