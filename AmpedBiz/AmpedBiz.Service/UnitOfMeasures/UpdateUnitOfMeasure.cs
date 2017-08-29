using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class UpdateUnitOfMeasure
    {
        public class Request : Dto.UnitOfMeasure, IRequest<Response> { }

        public class Response : Dto.UnitOfMeasure { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<UnitOfMeasure>(message.Id);
                    entity.EnsureExistence($"Unit of Measure with id {message.Id} does not exists.");
                    entity.MapFrom(message);
                    entity.EnsureValidity();

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
