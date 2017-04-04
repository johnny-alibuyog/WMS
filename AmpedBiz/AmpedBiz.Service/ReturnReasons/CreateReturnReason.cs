using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using AmpedBiz.Data;

namespace AmpedBiz.Service.ReturnReasons
{
    public class CreateReturnReason
    {
        public class Request : Dto.ReturnReason, IRequest<Response> { }

        public class Response : Dto.ReturnReason { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<ReturnReason>().Any(x => x.Id == message.Id);
                    exists.Assert($"Return Reason with id {message.Id} already exists.");

                    var entity = message.MapTo(new ReturnReason(message.Id));
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
