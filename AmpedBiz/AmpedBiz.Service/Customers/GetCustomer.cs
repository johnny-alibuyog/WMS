﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Customers
{
    public class GetCustomer
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

        public class Response : Dto.Customer { }

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
                    var entity = session.Get<Customer>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Customer with id {message.Id} does not exists.");

                    Mapper.Map<Customer, Dto.Customer>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}