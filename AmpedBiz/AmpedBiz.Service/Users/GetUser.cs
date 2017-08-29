using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
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
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
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
                    response.Password = entity.Password;
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