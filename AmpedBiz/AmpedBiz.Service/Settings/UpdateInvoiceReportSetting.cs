using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Settings;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Settings
{
	public class UpdateInvoiceReportSetting
	{
		public class Request : Dto.InvoiceReportSetting, IRequest<Response> { }

		public class Response : Dto.InvoiceReportSetting { }

		public class Handler : RequestHandlerBase<Request, Response>
		{
			public override Response Execute(Request message)
			{
				var response = new Response();

				using (var session = SessionFactory.RetrieveSharedSession(Context))
				using (var transaction = session.BeginTransaction())
				{
					var entity = session.Query<Setting<InvoiceReportSetting>>().FirstOrDefault();

					if (entity == null)
					{
						entity = new Setting<InvoiceReportSetting>()
						{
							Value = new InvoiceReportSetting()
							{
								PageItemSize = 6
							}
						};

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
