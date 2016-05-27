﻿using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.ProductCategories
{
    public class CreateProductCategory
    {
        public class Request : Dto.ProductCategory, IRequest<Response> { }

        public class Response : Dto.ProductCategory { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<ProductCategory>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Product Category with id {message.Id} already exists.");

                    var entity = message.MapTo(new ProductCategory(message.Id));

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
