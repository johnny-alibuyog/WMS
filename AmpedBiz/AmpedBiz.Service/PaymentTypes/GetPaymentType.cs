using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Data;
using MediatR;

namespace AmpedBiz.Service.PaymentTypes
{
	public class GetPaymentType
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response : Dto.PaymentType { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PaymentType>(message.Id);
                    entity.EnsureExistence($"Payment Type with id {message.Id} does not exists.");
                    entity.MapTo(response);

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
