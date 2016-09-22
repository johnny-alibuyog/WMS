using AmpedBiz.Service.ReturnReasons;
using MediatR;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("return-reasons")]
    public class ReturnReasonController : ApiController
    {
        private readonly IMediator _mediator;

        public ReturnReasonController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetReturnReason.Response Process([FromUri]GetReturnReason.Request request)
        {
            return _mediator.Send(request ?? new GetReturnReason.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetReturnReasonList.Response Process([FromUri]GetReturnReasonList.Request request)
        {
            return _mediator.Send(request ?? new GetReturnReasonList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetReturnReasonPage.Response Process([FromBody]GetReturnReasonPage.Request request)
        {
            return _mediator.Send(request ?? new GetReturnReasonPage.Request());
        }

        [HttpGet()]
        [Route("~/return-reason-lookups")]
        public GetReturnReasonLookup.Response Process([FromBody]GetReturnReasonLookup.Request request)
        {
            return _mediator.Send(request ?? new GetReturnReasonLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateReturnReason.Response Process([FromBody]CreateReturnReason.Request request)
        {
            return _mediator.Send(request ?? new CreateReturnReason.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateReturnReason.Response Process([FromBody]UpdateReturnReason.Request request)
        {
            return _mediator.Send(request ?? new UpdateReturnReason.Request());
        }
    }
}
