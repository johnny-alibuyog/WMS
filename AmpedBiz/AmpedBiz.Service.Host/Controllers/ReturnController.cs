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

        [HttpPost()]
        [Route("~/returns-by-customer/page")]
        public GetReturnsByCustomerPage.Response Process([FromBody]GetReturnsByCustomerPage.Request request)
        {
            return _mediator.Send(request ?? new GetReturnsByCustomerPage.Request());
        }

        [HttpPost()]
        [Route("~/returns-by-product/page")]
        public GetReturnsByProductPage.Response Process([FromBody]GetReturnsByProductPage.Request request)
        {
            return _mediator.Send(request ?? new GetReturnsByProductPage.Request());
        }

        [HttpPost()]
        [Route("~/returns-by-reason/page")]
        public GetReturnsByReasonPage.Response Process([FromBody]GetReturnsByReasonPage.Request request)
        {
            return _mediator.Send(request ?? new GetReturnsByReasonPage.Request());
        }
    }
}