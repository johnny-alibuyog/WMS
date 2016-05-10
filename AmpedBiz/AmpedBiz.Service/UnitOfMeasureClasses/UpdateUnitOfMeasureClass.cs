﻿using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class UpdateUnitOfMeasureClass
    {
        public class Request : Dto.UnitOfMeasureClass, IRequest<Response> { }

        public class Response : Dto.UnitOfMeasureClass { }

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
                    var entity = session.Get<Entity.UnitOfMeasureClass>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Class with id {message.Id} does not exists.");

                    Mapper.Map<Dto.UnitOfMeasureClass, Entity.UnitOfMeasureClass>(message, entity);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
