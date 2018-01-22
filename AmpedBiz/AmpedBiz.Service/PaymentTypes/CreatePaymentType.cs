using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.PaymentTypes
{
    public class CreatePaymentType
    {
        public class Request : Dto.PaymentType, IRequest<Response> { }

        public class Response : Dto.PaymentType { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<PaymentType>().Any(x => x.Id == message.Id);
                    exists.Assert($"Payment Type with id {message.Id} already exists.");

                    var entity = message.MapTo(new PaymentType(message.Id));
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
