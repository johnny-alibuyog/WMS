using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Settings
{
    public class GetInvoiceReportSetting
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

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
                        entity = Setting<InvoiceReportSetting>.Default();
                        session.Save(entity);
                    }

                    entity.Value.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
