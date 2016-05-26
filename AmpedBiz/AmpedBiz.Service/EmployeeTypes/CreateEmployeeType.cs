using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.EmployeeTypes
{
    public class CreateEmployeeType
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
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<EmployeeType>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Employee Type with id {message.Id} already exists.");

                    var entity = message.MapTo(default(EmployeeType));

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
