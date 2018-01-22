using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Users;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Users
{
    public class CreateUser
    {
        public class Request : Dto.User, IRequest<Response> { }

        public class Response : Dto.User { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = sessionFactory.RetrieveSharedSession(context))
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<User>().Any(x => x.Username == message.Username);
                    exists.Assert($"Username {message.Username} already exists.");

                    var roles = message.Roles
                        .Where(x => x.Assigned)
                        .Select(x => session.Get<Role>(x.Id))
                        .ToList();
                    var entity = new User();
                    entity.Username = message.Username;
                    entity.Person = message.Person.MapTo(default(Person));
                    entity.Address = message.Address.MapTo(default(Address));
                    entity.Branch = session.Get<Branch>(message.BranchId);
                    entity.Accept(new SetRoleVisitor(roles));
                    entity.Accept(new SetPasswordVisitor() 
                    {
                        // use username on creation. 
                        // the user himself/herself would be the one 
                        // to set the password on when he logs in
                        NewPassword = message.Username,
                        ConfirmPassword = message.Username
                    });
                    entity.EnsureValidity();

                    session.Save(entity);

                    transaction.Commit();

                    message.MapTo(response);

                }

                return response;
            }
        }
    }
}
