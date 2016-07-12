﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductList
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }

            public string SupplierId { get; set; }
        }

        public class Response : List<Dto.Product>
        {
            public Response() { }

            public Response(List<Dto.Product> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Product>();

                    if (!message.Id.IsNullOrEmpty())
                        query = query.Where(x => message.Id.Contains(x.Id));

                    if (!string.IsNullOrWhiteSpace(message.SupplierId))
                        query = query.Where(x => x.Supplier.Id == message.SupplierId);

                    var entities = query.ToList();

                    var dtos = entities.MapTo(default(List<Dto.Product>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}