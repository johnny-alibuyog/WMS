using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.ProductCategories
{
    public class GetProductCategoryLookup
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
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var pairs = session.Query<ProductCategory>()
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
