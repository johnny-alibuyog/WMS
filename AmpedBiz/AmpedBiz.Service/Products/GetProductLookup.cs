using AmpedBiz.Core.Entities;
using AmpedBiz.Common.Extentions;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Common.CustomTypes;

namespace AmpedBiz.Service.Products
{
    public class GetProductLookup
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }

            public string SupplierId { get; set; }
        }

        public class Response : List<Lookup<string>>
        {
            public Response() { }

            public Response(IList<Lookup<string>> items) : base(items) { }
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

                    var pairs = query
                        .Select(x => new Lookup<string>()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .Cacheable()
                        .ToList();

                    response = new Response(pairs);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
