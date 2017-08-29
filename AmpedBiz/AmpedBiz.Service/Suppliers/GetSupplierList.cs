using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Suppliers
{
    public class GetSupplierList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }
        }

        public class Response : List<Dto.Supplier>
        {
            public Response() { }

            public Response(List<Dto.Supplier> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entities = session.Query<Supplier>().Cacheable().ToList();
                    var dtos = entities.MapTo(default(List<Dto.Supplier>));

                    response = new Response(dtos);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
