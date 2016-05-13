using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

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
                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<EmployeeType>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Employee Type with id {message.Id} already exists.");

                    session.Save(new EmployeeType(message.Id, message.Name));

                    transaction.Commit();
                }

                return new Response()
                {
                    Id = message.Id,
                    Name = message.Name
                };
            }
        }
    }
}
