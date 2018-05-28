using AmpedBiz.Service.Customers;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("customers")]
    public class CustomerController : ApiController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public async Task<GetCustomer.Response> Process([FromUri]GetCustomer.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomer.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetCustomerList.Response> Process([FromUri]GetCustomerList.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetCustomerPage.Response> Process([FromBody]GetCustomerPage.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerPage.Request());
        }

        [HttpPost()]
        [Route("report/page")]
        public async Task<GetCustomerReportPage.Response> Process([FromBody]GetCustomerReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerReportPage.Request());
        }

        [HttpPost()]
        [Route("sales-report/page")]
        public async Task<GetCustomerSalesReportPage.Response> Process([FromBody]GetCustomerSalesReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerSalesReportPage.Request());
        }

        [HttpPost()]
        [Route("payments-report/page")]
        public async Task<GetCustomerPaymentsReportPage.Response> Process([FromBody]GetCustomerPaymentsReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerPaymentsReportPage.Request());
        }

        [HttpPost()]
        [Route("order-delivery-report/page")]
        public async Task<GetCustomerOrderDeliveryReportPage.Response> Process([FromBody]GetCustomerOrderDeliveryReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerOrderDeliveryReportPage.Request());
        }

        [HttpGet()]
        [Route("~/customer-lookups")]
        public async Task<GetCustomerLookup.Response> Process([FromBody]GetCustomerLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetCustomerLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateCustomer.Response> Process([FromBody]CreateCustomer.Request request)
        {
            return await _mediator.Send(request ?? new CreateCustomer.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateCustomer.Response> Process([FromBody]UpdateCustomer.Request request)
        {
            return await _mediator.Send(request ?? new UpdateCustomer.Request());
        }
    }
}
