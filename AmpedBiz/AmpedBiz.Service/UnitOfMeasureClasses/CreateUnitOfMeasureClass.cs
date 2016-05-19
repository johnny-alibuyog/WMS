﻿using System.Linq;
using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.UnitOfMeasureClasses
{
    public class CreateUnitOfMeasureClass
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
                    var exists = session.Query<Entity.UnitOfMeasureClass>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Class with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.UnitOfMeasureClass, Entity.UnitOfMeasureClass>(message, new Entity.UnitOfMeasureClass(message.Id));

                    session.Save(entity);

                    Mapper.Map<Entity.UnitOfMeasureClass, Dto.UnitOfMeasureClass>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}