using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.ReturnReasons
{
    public class GetReturnReasonLookup
    {
        public class Request : IRequest<Response>
        {
            public string[] Id { get; set; }
        }

        public class Response : List<Lookup<string>>
        {
            public Response() { }

            public Response(IList<Lookup<string>> items) : base(items) { }
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
                    var pairs = session.Query<ReturnReason>()
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
