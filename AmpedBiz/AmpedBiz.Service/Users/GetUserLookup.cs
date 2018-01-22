using AmpedBiz.Common.CustomTypes;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUserLookup
    {
        public class Request : IRequest<Response>
        {
            public Guid[] Id { get; set; }
        }

        public class Response : List<Lookup<Guid>>
        {
            public Response() { }

            public Response(IList<Lookup<Guid>> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var pairs = session.Query<User>()
                        .Select(x => new Lookup<Guid>()
                        {
                            Id = x.Id,
                            Name = 
                                x.Person.FirstName + " " +
                                x.Person.LastName
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
