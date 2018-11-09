using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.UnitOfMeasures
{
    public class CreateUnitOfMeasure
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
                    var exists = session.Query<UnitOfMeasure>().Any(x => x.Id == message.Id);
                    exists.Assert($"Unit of Measure with id {message.Id} already exists.");

                    var entity = message.MapTo(new UnitOfMeasure(message.Id));
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
