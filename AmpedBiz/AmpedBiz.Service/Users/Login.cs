﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Users;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Users
{
    public class Login
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
                    var user = session.QueryOver<User>()
                        .Where(x => x.Username == message.Username)
                        .Fetch(x => x.Roles).Eager
                        .Fetch(x => x.Branch).Eager
                        .Fetch(x => x.Branch.Tenant).Eager
                        .SingleOrDefault();

                    user.Ensure(
                        that: instance =>
                        {
                            if (instance == null)
                                return false;

                            var verfied = default(bool);

                            instance.Accept(new VerifyPasswordVisitor()
                            {
                                Password = message.Password,
                                ResultCallback = (result) => verfied = result
                            });

                            return verfied;
                        },
                        message: "Invalid user or password!"
                    );

                    user.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
