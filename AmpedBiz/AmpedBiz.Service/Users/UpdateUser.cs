using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Users;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class UpdateUser
    {
        public class Request : Dto.User, IRequest<Response> { }

        public class Response : Dto.User { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {

                    var roles = message.Roles
                        .Where(x => x.Assigned)
                        .Select(x => session.Get<Role>(x.Id))
                        .ToList();
                    var entity = session.Get<User>(message.Id);
                    entity.EnsureExistence($"User with id {message.Id} does not exists.");
                    entity.Username = message.Username;
                    entity.Person = message.Person.MapTo(default(Person));
                    entity.Address = message.Address.MapTo(default(Address));
                    entity.Branch = session.Get<Branch>(message.BranchId);
                    entity.Accept(new SetRoleVisitor(roles));

                    entity.EnsureValidity();

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
