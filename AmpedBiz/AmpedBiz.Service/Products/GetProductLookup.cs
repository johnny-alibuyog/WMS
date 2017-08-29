using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProductLookup
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }

            public Guid SupplierId { get; set; }
        }

        public class Response : List<Lookup<Guid>>
        {
            public Response() { }

            public Response(IList<Lookup<Guid>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {

                    var query = session.Query<Product>();

                    if (!message.Id.IsNullOrEmpty())
                        query = query.Where(x => message.Id.Contains(x.Id));

                    if (message.SupplierId != Guid.Empty)
                        query = query.Where(x => x.Supplier.Id == message.SupplierId);

                    var pairs = query
                        .Select(x => new Lookup<Guid>()
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .OrderBy(x => x.Name)
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
