using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Suppliers
{
    public class UpdateSupplier
    {
        public class Request : Dto.Supplier, IRequest<Response> { }

        public class Response : Dto.Supplier { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Supplier>(message.Id);
                    entity.EnsureExistence($"Supplier with id {message.Id} does not exists.");
                    entity.MapFrom(message);
                    entity.EnsureValidity();

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
