using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Products;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Pricings
{
	public class GetPricingLookup
    {
        public class Request : IRequest<Response> { }

        public class Response : List<Lookup<string>>
        {
            public Response() { }

            public Response(IList<Lookup<string>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {

                    var query = session.Query<Pricing>();

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

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
