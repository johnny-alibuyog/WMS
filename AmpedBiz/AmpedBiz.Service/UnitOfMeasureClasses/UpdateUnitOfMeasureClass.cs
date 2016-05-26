using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class UpdateUnitOfMeasureClass
    {
        public class Request : Dto.UnitOfMeasureClass, IRequest<Response> { }

        public class Response : Dto.UnitOfMeasureClass { }

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
                    var entity = session.Get<UnitOfMeasureClass>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Class with id {message.Id} does not exists.");

                    message.MapTo(entity);

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
