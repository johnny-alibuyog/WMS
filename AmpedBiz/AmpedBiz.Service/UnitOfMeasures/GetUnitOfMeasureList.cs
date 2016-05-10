using System.Collections.Generic;
using System.Linq;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class GetUnitOfMeasureList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.UnitOfMeasure>
        {
            public Response() { }

            public Response(List<Dto.UnitOfMeasure> items) : base(items) { }
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
                    var entites = session.Query<Entity.UnitOfMeasure>()
                        .ToList();

                    var result = Mapper.Map<List<Entity.UnitOfMeasure>, List<Dto.UnitOfMeasure>>(entites);

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
