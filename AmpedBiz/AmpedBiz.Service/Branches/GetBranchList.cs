using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Branches
{
    public class GetBranchList
    {
        public class Request : IRequest<Response>
        {
            public string[] Ids { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }

        public class Response : List<Dto.Branch>
        {
            public Response() { }

            public Response(List<Dto.Branch> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entities = session.Query<Branch>().ToList();
                    var dtos = entities.MapTo(default(List<Dto.Branch>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
