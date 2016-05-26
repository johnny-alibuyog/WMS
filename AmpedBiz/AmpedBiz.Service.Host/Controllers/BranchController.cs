using System.Web.Http;
using AmpedBiz.Service.Branches;
using Common.Logging;
using MediatR;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("branches")]
    public class BranchController : ApiController
    {
        private readonly ILog _log;
        private readonly IMediator _mediator;

        public BranchController(ILog log, IMediator mediator)
        {
            _log = log;
            _mediator = mediator;

            _log.Warn("log me like you do, lo log me like you do.");
            _log.Error("log me like you do, lo log me like you do.");
            _log.Fatal("log me like you do, lo log me like you do.");
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetBranch.Response Get([FromUri]GetBranch.Request request)
        {
            return _mediator.Send(request ?? new GetBranch.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetBranchList.Response Get([FromUri]GetBranchList.Request request)
        {
            return _mediator.Send(request ?? new GetBranchList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetBranchPage.Response Page([FromBody]GetBranchPage.Request request)
        {
            return _mediator.Send(request ?? new GetBranchPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateBranch.Response Create([FromBody]CreateBranch.Request request)
        {
            return _mediator.Send(request ?? new CreateBranch.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateBranch.Response Update([FromBody]UpdateBranch.Request request)
        {
            return _mediator.Send(request ?? new UpdateBranch.Request());
        }
    }
}