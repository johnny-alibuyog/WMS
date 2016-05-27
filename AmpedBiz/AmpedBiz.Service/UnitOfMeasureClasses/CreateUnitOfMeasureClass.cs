using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class CreateUnitOfMeasureClass
    {
        public class Request : Dto.UnitOfMeasureClass, IRequest<Response> { }

        public class Response : Dto.UnitOfMeasureClass { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<UnitOfMeasureClass>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Class with id {message.Id} already exists.");

                    var entity = message.MapTo(new UnitOfMeasureClass(message.Id));

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
