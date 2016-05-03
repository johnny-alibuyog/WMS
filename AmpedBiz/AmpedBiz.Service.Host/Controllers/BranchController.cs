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
        public GetBranches.Response Get([FromUri]GetBranches.Request request)
        {
            return _mediator.Send(request ?? new GetBranches.Request());
        }

        [HttpPost()]
        [Route("pages")]
        public GetBranchePages.Response Page([FromBody]GetBranchePages.Request request)
        {
            return _mediator.Send(request ?? new GetBranchePages.Request());
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