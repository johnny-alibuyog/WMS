using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using System.Linq;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;
using NHibernate.Transform;

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
                    var entity = session.QueryOver<Entity.User>()
                        .Where(x => x.Id == message.Id)
                        .Left.JoinQueryOver(x => x.UserRoles)
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .SingleOrDefault();

                    //var entity = query.Value;
                    var roles = session.Query<Entity.Role>().ToList();

                    response.Id = entity.Id;
                    response.Username = entity.Username;
                    response.Password = entity.Password;
                    response.Person = Mapper.Map<Entity.Person, Dto.Person>(entity.Person);
                    response.Address = Mapper.Map<Entity.Address, Dto.Address>(entity.Address);
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