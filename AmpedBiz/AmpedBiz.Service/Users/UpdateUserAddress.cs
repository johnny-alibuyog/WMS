using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using System;

namespace AmpedBiz.Service.Users
{
    public class UpdateUserAddress
    {
        public class Request : Dto.UserAddress, IRequest<Response> { }

        public class Response { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<User>(message.Id);

                    entity.EnsureExistence($"User with id {message.Id} does not exists.");

                    entity.Address = message.Address.MapTo(default(Address));

                    entity.EnsureValidity();

                    transaction.Commit();

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
