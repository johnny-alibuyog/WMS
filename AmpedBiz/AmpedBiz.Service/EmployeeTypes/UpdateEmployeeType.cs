using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.EmployeeTypes
{
    public class UpdateEmployeeType
    {
        public class Request : Dto.EmployeeType, IRequest<Response> { }

        public class Response : Dto.EmployeeType { }

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
                    var entity = session.Get<EmployeeType>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Employee Type with id {message.Id} does not exists.");

                    entity.Name = message.Name;

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
