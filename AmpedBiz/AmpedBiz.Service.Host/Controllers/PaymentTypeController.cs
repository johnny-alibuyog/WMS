using AmpedBiz.Service.PaymentTypes;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("payment-types")]
    public class PaymentTypeController : ApiController
    {
        private readonly IMediator _mediator;

        public PaymentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public async Task<GetPaymentType.Response> Process([FromUri]GetPaymentType.Request request)
        {
            return await _mediator.Send(request ?? new GetPaymentType.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetPaymentTypeList.Response> Process([FromUri]GetPaymentTypeList.Request request)
        {
            return await _mediator.Send(request ?? new GetPaymentTypeList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetPaymentTypePage.Response> Process([FromBody]GetPaymentTypePage.Request request)
        {
            return await _mediator.Send(request ?? new GetPaymentTypePage.Request());
        }

        [HttpGet()]
        [Route("~/payment-type-lookups")]
        public async Task<GetPaymentTypeLookup.Response> Process([FromBody]GetPaymentTypeLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetPaymentTypeLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreatePaymentType.Response> Process([FromBody]CreatePaymentType.Request request)
        {
            return await _mediator.Send(request ?? new CreatePaymentType.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdatePaymentType.Response> Process([FromBody]UpdatePaymentType.Request request)
        {
            return await _mediator.Send(request ?? new UpdatePaymentType.Request());
        }
    }
}
