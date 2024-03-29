﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Employees
{
    public class GetEmployee
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response : Dto.Employee { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Employee>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Employee with id {message.Id} does not exists.");

                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}