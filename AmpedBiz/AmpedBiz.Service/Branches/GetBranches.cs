using System.Collections.Generic;
using System.Linq;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Branches
{
    public class GetBranches
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.Branch>
        {
            public Response() { }

            public Response(List<Dto.Branch> items) : base(items) { }
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
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entites = session.Query<Entity.Branch>()
                        .Select(x => new Dto.Branch()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToList();

                    response = new Response(entites);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
