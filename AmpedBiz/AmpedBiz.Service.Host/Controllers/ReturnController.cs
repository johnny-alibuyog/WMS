using AmpedBiz.Service.Returns;
using MediatR;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("returns")]
    public class ReturnController : ApiController
    {
        private readonly IMediator _mediator;

        public ReturnController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetReturn.Response Process([FromUri]GetReturn.Request request)
        {
            return _mediator.Send(request ?? new GetReturn.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateReturn.Response Process([FromBody]CreateReturn.Request request)
        {
            return _mediator.Send(request ?? new CreateReturn.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetReturnList.Response Process([FromUri]GetReturnList.Request request)
        {
            return _mediator.Send(request ?? new GetReturnList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetReturnPage.Response Process([FromBody]GetReturnPage.Request request)
        {
            return _mediator.Send(request ?? new GetReturnPage.Request());
        }
    }
}