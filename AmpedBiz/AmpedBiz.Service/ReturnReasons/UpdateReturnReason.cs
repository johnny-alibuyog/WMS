using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.ReturnReasons
{
    public class UpdateReturnReason
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
                    var entity = session.Get<ReturnReason>(message.Id);
                    entity.EnsureExistence($"Return Reason with id {message.Id} does not exists.");
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
