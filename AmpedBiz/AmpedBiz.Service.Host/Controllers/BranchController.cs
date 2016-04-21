using System.Web.Http;
using AmpedBiz.Service.Branches;
using MediatR;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("branches")]
    public class BranchController : ApiController
    {
        private readonly IMediator _mediator;

        public BranchController(IMediator mediator)
        {
            _mediator = mediator;
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
        [Route("page")]
        public GetBranches.Response Page([FromBody]GetBranches.Request request)
        {
            return _mediator.Send(request ?? new GetBranches.Request());
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