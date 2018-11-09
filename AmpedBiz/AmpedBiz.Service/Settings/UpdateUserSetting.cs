using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Settings;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Settings
{
	public class UpdateUserSetting
    {
        public class Request : Dto.UserSetting, IRequest<Response> { }

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

                    entity.Value.MapFrom(message);

                    entity.EnsureValidity();

                    transaction.Commit();

                    entity.Value.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
