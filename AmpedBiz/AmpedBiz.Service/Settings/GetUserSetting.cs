﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Settings;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Settings
{
	public class GetUserSetting
	{
		public class Request : IRequest<Response>
		{
			public Guid Id { get; set; }
		}

		public class Response : Dto.UserSetting { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entity = session.Query<Setting<UserSetting>>().FirstOrDefault();
					if (entity == null)
					{
						entity = Setting<UserSetting>.Default();
						session.Save(entity);
					}

					entity.Value.MapTo(response);

					transaction.Commit();

					SessionFactory.ReleaseSharedSession();
				}

				return response;
			}
		}
	}
}
