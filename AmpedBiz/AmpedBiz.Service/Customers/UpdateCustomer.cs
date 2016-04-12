﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Customers
{
    public class UpdateCustomer
    {
        public class Request : Dto.Customer, IRequest<Response> { }

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

                    Mapper.Map<Dto.Customer, Customer>(message, entity);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}