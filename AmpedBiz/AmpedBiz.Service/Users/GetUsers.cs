using System.Collections.Generic;
using System.Linq;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Users
{
    public class GetUsers
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response : List<Dto.User>
        {
            public Response() { }

            public Response(List<Dto.User> items) : base(items) { }
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
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entities = session.Query<Entity.User>().ToList();

                    var result = entities
                        .Select(x => new Dto.User()
                        {
                            Id = x.Id,
                            Username = x.Username,
                            Password = x.Password,
                            Person = Mapper.Map<Entity.Person, Dto.Person>(x.Person),
                            Address = Mapper.Map<Entity.Address, Dto.Address>(x.Address),
                            BranchId = x.Branch.Id
                        })
                        .ToList();

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}