using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Suppliers
{
    public class CreateSupplier
    {
        public class Request : Dto.Supplier, IRequest<Response> { }

        public class Response : Dto.Supplier { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Supplier>().Any(x => x.Id == message.Id);

                    exists.Assert($"Supplier with id {message.Id} already exists.");

                    var entity = message.MapTo(new Supplier(message.Id));

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
