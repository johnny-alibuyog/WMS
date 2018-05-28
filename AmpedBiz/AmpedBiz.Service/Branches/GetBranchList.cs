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

namespace AmpedBiz.Service.Branches
{
    public class GetBranchList
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Ids { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }
        }

        public class Response : List<Dto.Branch>
        {
            public Response() { }

            public Response(List<Dto.Branch> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entities = session.Query<Branch>().Cacheable().ToList();
                    var dtos = entities.MapTo(default(List<Dto.Branch>));

                    response = new Response(dtos);

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
