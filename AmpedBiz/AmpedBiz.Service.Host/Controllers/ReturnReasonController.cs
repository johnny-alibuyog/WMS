using AmpedBiz.Service.ReturnReasons;
using MediatR;
using System.Threading.Tasks;
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
        public async Task<GetReturnReason.Response> Process([FromUri]GetReturnReason.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnReason.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetReturnReasonList.Response> Process([FromUri]GetReturnReasonList.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnReasonList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetReturnReasonPage.Response> Process([FromBody]GetReturnReasonPage.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnReasonPage.Request());
        }

        [HttpGet()]
        [Route("~/return-reason-lookups")]
        public async Task<GetReturnReasonLookup.Response> Process([FromBody]GetReturnReasonLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnReasonLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateReturnReason.Response> Process([FromBody]CreateReturnReason.Request request)
        {
            return await _mediator.Send(request ?? new CreateReturnReason.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateReturnReason.Response> Process([FromBody]UpdateReturnReason.Request request)
        {
            return await _mediator.Send(request ?? new UpdateReturnReason.Request());
        }
    }
}
