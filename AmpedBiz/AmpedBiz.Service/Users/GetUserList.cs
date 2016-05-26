using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUserList
    {
        public class Request : IRequest<Response>
        {
            public string[] Ids { get; set; }
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
                    var entities = session.Query<User>().ToList();

                    var result = entities
                        .Select(x => new Dto.User()
                        {
                            Id = x.Id,
                            Username = x.Username,
                            Password = x.Password,
                            Person = x.Person.MapTo(default(Dto.Person)),
                            Address = x.Address.MapTo(default(Dto.Address)),
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