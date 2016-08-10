using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.PricingShemes
{
    public class GetPricingSchemeLookup
    {
        public class Request : IRequest<Response> { }

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

                    var query = session.Query<PricingScheme>();

                    var pairs = query
                        .Select(x => new Lookup<string>()
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
