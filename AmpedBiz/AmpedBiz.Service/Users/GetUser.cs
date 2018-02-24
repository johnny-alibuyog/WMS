using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class GetUser
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : Dto.User { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.QueryOver<User>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Roles).Eager
                        .SingleOrDefault();

                    //var entity = query.Value;
                    var roles = session.Query<Role>().Cacheable().ToList();

                    response.Id = entity.Id;
                    response.Username = entity.Username;
                    response.Person = entity.Person.MapTo(default(Dto.Person));
                    response.Address = entity.Address.MapTo(default(Dto.Address));
                    response.BranchId = entity.Branch.Id;
                    response.Roles = roles
                        .Select(x => new Dto.Role()
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Assigned = entity.Roles
                                .Select(o => o.Id)
                                .Contains(x.Id)
                        })
                        .ToList();

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}