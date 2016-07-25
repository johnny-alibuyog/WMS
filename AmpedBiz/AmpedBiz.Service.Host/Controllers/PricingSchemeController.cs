using AmpedBiz.Service.PricingShemes;
using MediatR;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("pricing-schemes")]
    public class PricingSchemeController : ApiController
    {
        private readonly IMediator _mediator;

        public PricingSchemeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("~/pricing-scheme-lookups")]
        public GetPricingSchemeLookup.Response Process([FromUri]GetPricingSchemeLookup.Request request)
        {
            return _mediator.Send(request ?? new GetPricingSchemeLookup.Request());
        }
    }
}