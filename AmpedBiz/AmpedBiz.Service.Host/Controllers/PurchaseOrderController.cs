using AmpedBiz.Service.PurchaseOrders;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("purchase-orders")]
    public class PurchaseOrderController : ApiController
    {
        private readonly IMediator _mediator;

        public PurchaseOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public async Task<GetPurchaseOrder.Response> Process([FromUri]GetPurchaseOrder.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrder.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetPurchaseOrderList.Response> Process([FromUri]GetPurchaseOrderList.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrderList.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<SavePurchaseOrder.Response> Process([FromBody]SavePurchaseOrder.Request request)
        {
            return await _mediator.Send(request ?? new SavePurchaseOrder.Request());
        }

        [HttpPut()]
        [Route("{id}")]
        public async Task<SavePurchaseOrder.Response> Process([FromUri]Guid id, [FromBody]SavePurchaseOrder.Request request)
        {
            return await _mediator.Send(request ?? new SavePurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetPurchaseOrderPage.Response> Process([FromBody]GetPurchaseOrderPage.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrderPage.Request());
        }

		[HttpPost()]
		[Route("report/page")]
		public async Task<GetPurchaseOrderReportPage.Response> Process([FromBody]GetPurchaseOrderReportPage.Request request)
		{
			return await _mediator.Send(request ?? new GetPurchaseOrderReportPage.Request());
		}

		[HttpGet()]
        [Route("statuses")]
        public async Task<GetPurchaseOrderStatusList.Response> Process([FromBody]GetPurchaseOrderStatusList.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrderStatusList.Request());
        }

        [HttpGet()]
        [Route("status-lookups")]
        public async Task<GetPurchaseOrderStatusLookup.Response> Process([FromUri]GetPurchaseOrderStatusLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrderStatusLookup.Request());
        }

        [HttpGet()]
        [Route("{id}/payables")]
        public async Task<GetPurchaseOrderPayable.Response> Process([FromUri]GetPurchaseOrderPayable.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrderPayable.Request());
        }

        [HttpGet()]
        [Route("{id}/voucher")]
        public async Task<GetVoucher.Response> Process([FromUri]GetVoucher.Request request)
        {
            return await _mediator.Send(request ?? new GetVoucher.Request());
        }

        [HttpPost()]
        [Route("active-orders/page")]
        public async Task<GetActivePurchaseOrderPage.Response> Process([FromBody]GetActivePurchaseOrderPage.Request request)
        {
            return await _mediator.Send(request ?? new GetActivePurchaseOrderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/submitted")]
        public async Task<SubmitPurchaseOrder.Response> Process([FromUri]Guid id, [FromBody]SubmitPurchaseOrder.Request request)
        {
            return await _mediator.Send(request ?? new SubmitPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/approved")]
        public async Task<ApprovePurchaseOder.Response> Process([FromUri]Guid id, [FromBody]ApprovePurchaseOder.Request request)
        {
            return await _mediator.Send(request ?? new ApprovePurchaseOder.Request());
        }

        [HttpPost()]
        [Route("{id}/recreated")]
        public async Task<RecreatePurchaseOder.Response> Process([FromUri]Guid id, [FromBody]RecreatePurchaseOder.Request request)
        {
            return await _mediator.Send(request ?? new RecreatePurchaseOder.Request());
        }

        [HttpGet()]
        [Route("{id}/receivables")]
        public async Task<GetPurchaseOrderReceivableList.Response> Process([FromUri]GetPurchaseOrderReceivableList.Request request)
        {
            return await _mediator.Send(request ?? new GetPurchaseOrderReceivableList.Request());
        }

        [HttpPost()]
        [Route("{id}/cancelled")]
        public async Task<CancelPurchaseOder.Response> Process([FromUri]Guid id, [FromBody]CancelPurchaseOder.Request request)
        {
            return await _mediator.Send(request ?? new CancelPurchaseOder.Request());
        }

        [HttpPost()]
        [Route("{id}/completed")]
        public async Task<CompletePurchaseOder.Response> Process([FromUri]Guid id, [FromBody]CompletePurchaseOder.Request request)
        {
            return await _mediator.Send(request ?? new CompletePurchaseOder.Request());
        }
    }
}
