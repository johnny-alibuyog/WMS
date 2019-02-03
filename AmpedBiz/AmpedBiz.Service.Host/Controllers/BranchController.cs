using AmpedBiz.Service.Branches;
using Common.Logging;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task<GetBranch.Response> Process([FromUri]GetBranch.Request request)
        {
            return await _mediator.Send(request ?? new GetBranch.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetBranchList.Response> Process([FromUri]GetBranchList.Request request)
        {
            return await _mediator.Send(request ?? new GetBranchList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetBranchPage.Response> Process([FromBody]GetBranchPage.Request request)
        {
            return await _mediator.Send(request ?? new GetBranchPage.Request());
        }

        [HttpGet()]
        [Route("~/branch-lookups")]
        public async Task<GetBranchLookup.Response> Process([FromUri]GetBranchLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetBranchLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateBranch.Response> Process([FromBody]CreateBranch.Request request)
        {
            return await _mediator.Send(request ?? new CreateBranch.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateBranch.Response> Process([FromBody]UpdateBranch.Request request)
        {
            return await _mediator.Send(request ?? new UpdateBranch.Request());
        }
    }
}