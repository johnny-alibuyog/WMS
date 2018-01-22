using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Collections.Generic;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUserList
    {
        public class Request : IRequest<Response>
        {
            public string[] Ids { get; set; }
        }

        public class Response : List<Dto.User>
        {
            public Response() { }

            public Response(List<Dto.User> items) : base(items) { }
        }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var entities = session.Query<User>().Cacheable().ToList();

                    var result = entities
                        .Select(x => new Dto.User()
                        {
                            Id = x.Id,
                            Username = x.Username,
                            Person = x.Person.MapTo(default(Dto.Person)),
                            Address = x.Address.MapTo(default(Dto.Address)),
                            BranchId = x.Branch.Id
                        })
                        .ToList();

                    response = new Response(result);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}