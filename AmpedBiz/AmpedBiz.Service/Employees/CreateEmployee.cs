using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Employees
{
    public class CreateEmployee
    {
        public class Request : Dto.Employee, IRequest<Response> { }

        public class Response : Dto.Employee { }

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
                    var exists = session.Query<Employee>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Employee with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.Employee, Employee>(message, new Employee(message.Id));
                    entity.EmployeeType = session.Load<EmployeeType>(message.EmployeeTypeId);
                    entity.User = session.Load<User>(message.User.Id);

                    session.Save(entity);

                    Mapper.Map<Employee, Dto.Employee>(entity, response);
                    response.EmployeeTypeId = entity.EmployeeType.Id;
                    response.User.BranchId = entity.User.Branch.Id;
                    //todo: roles

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}