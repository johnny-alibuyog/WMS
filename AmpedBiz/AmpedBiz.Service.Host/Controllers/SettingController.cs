using AmpedBiz.Service.Settings;
using Common.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("settings")]
    public class SettingController : ApiController
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public SettingController(ILog log, IMediator mediator)
        {
            _log = log;
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("invoice-report")]
        public async Task<GetInvoiceReportSetting.Response> Process([FromUri]GetInvoiceReportSetting.Request request)
        {
            return await _mediator.Send(request ?? new GetInvoiceReportSetting.Request());
        }

        [HttpPost()]
        [Route("invoice-report")]
        public async Task<UpdateInvoiceReportSetting.Response> Process([FromBody]UpdateInvoiceReportSetting.Request request)
        {
            return await _mediator.Send(request ?? new UpdateInvoiceReportSetting.Request());
        }

        [HttpGet()]
        [Route("user")]
        public async Task<GetUserSetting.Response> Process([FromUri]GetUserSetting.Request request)
        {
            return await _mediator.Send(request ?? new GetUserSetting.Request());
        }

        [HttpPost()]
        [Route("user")]
        public async Task<UpdateUserSetting.Response> Process([FromBody]UpdateUserSetting.Request request)
        {
            return await _mediator.Send(request ?? new UpdateUserSetting.Request());
        }
    }
}