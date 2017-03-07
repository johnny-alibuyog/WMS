using AmpedBiz.Service.Pricings;
using MediatR;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("pricings")]
    public class PricingController : ApiController
    {
        private readonly IMediator _mediator;

        public PricingController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("~/pricing-lookups")]
        public GetPricingLookup.Response Process([FromUri]GetPricingLookup.Request request)
        {
            return _mediator.Send(request ?? new GetPricingLookup.Request());
        }
    }
}