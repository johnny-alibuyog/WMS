using AmpedBiz.Service.Returns;
using MediatR;
using System.Threading.Tasks;
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
        public async Task<GetReturn.Response> Process([FromUri]GetReturn.Request request)
        {
            return await _mediator.Send(request ?? new GetReturn.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateReturn.Response> Process([FromBody]CreateReturn.Request request)
        {
            return await _mediator.Send(request ?? new CreateReturn.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetReturnList.Response> Process([FromUri]GetReturnList.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetReturnPage.Response> Process([FromBody]GetReturnPage.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnPage.Request());
        }

		[HttpPost()]
		[Route("~/returns-details-report/page")]
		public async Task<GetReturnDetailsReportPage.Response> Process([FromBody]GetReturnDetailsReportPage.Request request)
		{
			return await _mediator.Send(request ?? new GetReturnDetailsReportPage.Request());
		}

		[HttpPost()]
        [Route("~/returns-by-customer/page")]
        public async Task<GetReturnsByCustomerPage.Response> Process([FromBody]GetReturnsByCustomerPage.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnsByCustomerPage.Request());
        }

		[HttpPost()]
		[Route("~/returns-by-customer-details/page")]
		public async Task<GetReturnsByCustomerDetailsPage.Response> Process([FromBody]GetReturnsByCustomerDetailsPage.Request request)
		{
			return await _mediator.Send(request ?? new GetReturnsByCustomerDetailsPage.Request());
		}

		[HttpPost()]
        [Route("~/returns-by-product/page")]
        public async Task<GetReturnsByProductPage.Response> Process([FromBody]GetReturnsByProductPage.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnsByProductPage.Request());
        }

		[HttpPost()]
		[Route("~/returns-by-product-details/page")]
		public async Task<GetReturnsByProductDetailsPage.Response> Process([FromBody]GetReturnsByProductDetailsPage.Request request)
		{
			return await _mediator.Send(request ?? new GetReturnsByProductDetailsPage.Request());
		}

		[HttpPost()]
        [Route("~/returns-by-reason/page")]
        public async Task<GetReturnsByReasonPage.Response> Process([FromBody]GetReturnsByReasonPage.Request request)
        {
            return await _mediator.Send(request ?? new GetReturnsByReasonPage.Request());
        }

		[HttpPost()]
		[Route("~/returns-by-reason-details/page")]
		public async Task<GetReturnsByReasonDetailsPage.Response> Process([FromBody]GetReturnsByReasonDetailsPage.Request request)
		{
			return await _mediator.Send(request ?? new GetReturnsByReasonDetailsPage.Request());
		}
	}
}