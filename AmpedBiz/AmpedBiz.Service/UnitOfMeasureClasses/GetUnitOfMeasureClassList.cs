using System.Collections.Generic;
using System.Linq;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class GetUnitOfMeasureClassList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Dto.UnitOfMeasureClass>
        {
            public Response() { }

            public Response(List<Dto.UnitOfMeasureClass> items) : base(items) { }
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
                    var entites = session
                        .Query<Entity.UnitOfMeasureClass>()
                        .Fetch(x => x.Units)
                        .ToList();

                    var result = Mapper.Map<List<Entity.UnitOfMeasureClass>, List<Dto.UnitOfMeasureClass>>(entites);

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
