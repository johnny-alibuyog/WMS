using AmpedBiz.Service.PurchaseOrders;
using MediatR;
using System;
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
        public GetPurchaseOrder.Response Process([FromUri]GetPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrder.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetPurchaseOrderList.Response Process([FromUri]GetPurchaseOrderList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderList.Request());
        }

        [HttpPost()]
        [Route("")]
        public SavePurchaseOrder.Response Process([FromBody]SavePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SavePurchaseOrder.Request());
        }

        [HttpPut()]
        [Route("{id}")]
        public SavePurchaseOrder.Response Process([FromUri]Guid id, [FromBody]SavePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SavePurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetPurchaseOrderPage.Response Process([FromBody]GetPurchaseOrderPage.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderPage.Request());
        }

        [HttpGet()]
        [Route("statuses")]
        public GetPurchaseOrderStatusList.Response Process([FromBody]GetPurchaseOrderStatusList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderStatusList.Request());
        }

        [HttpGet()]
        [Route("status-lookups")]
        public GetPurchaseOrderStatusLookup.Response Process([FromBody]GetPurchaseOrderStatusLookup.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderStatusLookup.Request());
        }

        [HttpGet()]
        [Route("{id}/payables")]
        public GetPurchaseOrderPayable.Response Process([FromUri]GetPurchaseOrderPayable.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderPayable.Request());
        }

        [HttpPost()]
        [Route("active-orders/page")]
        public GetActivePurchaseOrderPage.Response Process([FromBody]GetActivePurchaseOrderPage.Request request)
        {
            return _mediator.Send(request ?? new GetActivePurchaseOrderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/submitted")]
        public SubmitPurchaseOrder.Response Process([FromUri]Guid id, [FromBody]SubmitPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SubmitPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/approved")]
        public ApprovePurchaseOder.Response Process([FromUri]Guid id, [FromBody]ApprovePurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new ApprovePurchaseOder.Request());
        }

        [HttpGet()]
        [Route("{id}/receivables")]
        public GetPurchaseOrderReceivableList.Response Process([FromUri]GetPurchaseOrderReceivableList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderReceivableList.Request());
        }

        [HttpPost()]
        [Route("{id}/cancelled")]
        public CancelPurchaseOder.Response Process([FromUri]Guid id, [FromBody]CancelPurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CancelPurchaseOder.Request());
        }

        [HttpPost()]
        [Route("{id}/completed")]
        public CompletePurchaseOder.Response Process([FromUri]Guid id, [FromBody]CompletePurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CompletePurchaseOder.Request());
        }
    }
}
