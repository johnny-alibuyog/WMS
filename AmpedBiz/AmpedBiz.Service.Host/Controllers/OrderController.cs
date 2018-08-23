using AmpedBiz.Service.Orders;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("orders")]
    public class OrderController : ApiController
    {
        private readonly IMediator _mediator;

        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public async Task<GetOrder.Response> Process([FromUri]GetOrder.Request request)
        {
            return await _mediator.Send(request ?? new GetOrder.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetOrderList.Response> Process([FromUri]GetOrderList.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderList.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<SaveOrder.Response> Process([FromBody]SaveOrder.Request request)
        {
            return await _mediator.Send(request ?? new SaveOrder.Request());
        }

        [HttpPut()]
        [Route("{id}")]
        public async Task<SaveOrder.Response> Process([FromUri]Guid id, [FromBody]SaveOrder.Request request)
        {
            return await _mediator.Send(request ?? new SaveOrder.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetOrderPage.Response> Process([FromBody]GetOrderPage.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderPage.Request());
        }

        [HttpPost()]
        [Route("report/page")]
        public async Task<GetOrderReportPage.Response> Process([FromBody]GetOrderReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderReportPage.Request());
        }

        [HttpPost()]
        [Route("sales-report/page")]
        public async Task<GetSalesReportPage.Response> Process([FromBody]GetSalesReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetSalesReportPage.Request());
        }

        [HttpGet()]
        [Route("statuses")]
        public async Task<GetOrderStatusList.Response> Process([FromBody]GetOrderStatusList.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderStatusList.Request());
        }

        [HttpGet()]
        [Route("status-lookups")]
        public async Task<GetOrderStatusLookup.Response> Process([FromBody]GetOrderStatusLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderStatusLookup.Request());
        }

        [HttpGet()]
        [Route("{id}/payables")]
        public async Task<GetOrderPayable.Response> Process([FromUri]GetOrderPayable.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderPayable.Request());
        }

        [HttpGet()]
        [Route("{id}/returnables")]
        public async Task<GetOrderReturnable.Response> Process([FromUri]GetOrderReturnable.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderReturnable.Request());
        }

        [HttpGet()]
        [Route("{id}/invoice-detail")]
        public async Task<GetOrderInvoiceDetail.Response> Process([FromUri]GetOrderInvoiceDetail.Request request)
        {
            return await _mediator.Send(request ?? new GetOrderInvoiceDetail.Request());
        }

        [HttpPost()]
        [Route("active-orders/page")]
        public async Task<GetActiveOrderPage.Response> Process([FromBody]GetActiveOrderPage.Request request)
        {
            return await _mediator.Send(request ?? new GetActiveOrderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/staged")]
        public async Task<StageOrder.Response> Process([FromUri]Guid id, [FromBody]StageOrder.Request request)
        {
            return await _mediator.Send(request ?? new StageOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/routed")]
        public async Task<RouteOrder.Response> Process([FromUri]Guid id, [FromBody]RouteOrder.Request request)
        {
            return await _mediator.Send(request ?? new RouteOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/invoiced")]
        public async Task<InvoiceOrder.Response> Process([FromUri]Guid id, [FromBody]InvoiceOrder.Request request)
        {
            return await _mediator.Send(request ?? new InvoiceOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/modified-back")]
        public async Task<ModifyBackOrder.Response> Process([FromUri]Guid id, [FromBody]ModifyBackOrder.Request request)
        {
            return await _mediator.Send(request ?? new ModifyBackOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/shipped")]
        public async Task<ShipOrder.Response> Process([FromUri]Guid id, [FromBody]ShipOrder.Request request)
        {
            return await _mediator.Send(request ?? new ShipOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/completed")]
        public async Task<CompleteOrder.Response> Process([FromUri]Guid id, [FromBody]CompleteOrder.Request request)
        {
            return await _mediator.Send(request ?? new CompleteOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/cancelled")]
        public async Task<CancelOrder.Response> Process([FromUri]Guid id, [FromBody]CancelOrder.Request request)
        {
            return await _mediator.Send(request ?? new CancelOrder.Request());
        }
    }
}