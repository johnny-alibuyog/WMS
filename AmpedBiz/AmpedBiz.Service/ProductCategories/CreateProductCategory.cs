﻿using System.Linq;
using AmpedBiz.Common.Exceptions;
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
                    var exists = session.Query<ProductCategory>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Product Category with id {message.Id} already exists.");

                    session.Save(new ProductCategory()
                    {
                        Id = message.Id,
                        Name = message.Name
                    });

                    transaction.Commit();
                }

                return new Response()
                {
                    Id = message.Id,
                    Name = message.Id
                };
            }
        }
    }
}