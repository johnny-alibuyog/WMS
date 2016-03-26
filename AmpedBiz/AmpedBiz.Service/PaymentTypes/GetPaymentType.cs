using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.PaymentTypes
{
    public class GetPaymentType
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response : Dto.PaymentType { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PaymentType>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Payment Type with id {message.Id} does not exists.");

                    response = new Response()
                    {
                        Id = entity.Id,
                        Name = entity.Name
                    };

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
