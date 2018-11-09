using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Data;
using MediatR;

namespace AmpedBiz.Service.Customers
{
	public class UpdateCustomer
    {
        public class Request : Dto.Customer, IRequest<Response> { }

        public class Response : Dto.Customer { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Customer>(message.Id);
                    entity.EnsureExistence($"Customer with id {message.Id} does not exists.");
                    entity.MapFrom(message);
                    entity.EnsureValidity();

                    transaction.Commit();

                    entity.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}