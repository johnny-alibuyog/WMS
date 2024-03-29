﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Common;
using AmpedBiz.Core.Users;
using AmpedBiz.Core.Users.Services;
using AmpedBiz.Data;
using MediatR;
using System.Linq;

namespace AmpedBiz.Service.Users
{
	public class UpdateUser
	{
		public class Request : Dto.User, IRequest<Response> { }

		public class Response : Dto.User { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
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

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
