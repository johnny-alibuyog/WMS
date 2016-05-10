using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class UpdateUnitOfMeasure
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
                    var entity = session.Get<Entity.UnitOfMeasure>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Unit of Measure with id {message.Id} does not exists.");

                    Mapper.Map<Dto.UnitOfMeasure, Entity.UnitOfMeasure>(message, entity);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
