using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class UpdateUnitOfMeasure
    {
        public class Request : Dto.UnitOfMeasure, IRequest<Response> { }

        public class Response : Dto.UnitOfMeasure { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<UnitOfMeasure>(message.Id);
                    entity.EnsureExistence($"Unit of Measure with id {message.Id} does not exists.");
                    entity.MapFrom(message);
                    entity.EnsureValidity();

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
