using System.Linq;
using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class CreateUnitOfMeasure
    {
        public class Request : Dto.UnitOfMeasure, IRequest<Response> { }

        public class Response : Dto.UnitOfMeasure { }

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
                    var exists = session.Query<Entity.UnitOfMeasure>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Unit of Measure with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.UnitOfMeasure, Entity.UnitOfMeasure>(message, new Entity.UnitOfMeasure(message.Id));

                    session.Save(entity);

                    Mapper.Map<Entity.UnitOfMeasure, Dto.UnitOfMeasure>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
