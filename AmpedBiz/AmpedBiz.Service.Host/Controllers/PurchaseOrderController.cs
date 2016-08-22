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
        [Route("new")]
        public CreateNewPurchaseOder.Response Process([FromBody]CreateNewPurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CreateNewPurchaseOder.Request());
        }

        [HttpPatch()]
        [Route("{id}/new")]
        public UpdateNewPurchaseOder.Response Process([FromUri]Guid id, [FromBody]UpdateNewPurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new UpdateNewPurchaseOder.Request());
        }

        //[HttpPost()]
        //[Route("new/page")]
        //public GetNewPurchaseOderPage.Response Process([FromBody]GetNewPurchaseOderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetNewPurchaseOderPage.Request());
        //}

        //[HttpPost()]
        //[Route("active/page")]
        //public GetActivePurchaseOderPage.Response Process([FromBody]GetActivePurchaseOderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetActivePurchaseOderPage.Request());
        //}

        [HttpPost()]
        [Route("{id}/submitted")]
        public SubmitPurchaseOrder.Response Process([FromUri]Guid id, [FromBody]SubmitPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SubmitPurchaseOrder.Request());
        }

        //[HttpPost()]
        //[Route("submitted/page")]
        //public GetSubmittedPurchaseOderPage.Response Process([FromBody]GetSubmittedPurchaseOderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetSubmittedPurchaseOderPage.Request());
        //}

        [HttpPost()]
        [Route("{id}/approved")]
        public ApprovePurchaseOder.Response Process([FromUri]Guid id, [FromBody]ApprovePurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new ApprovePurchaseOder.Request());
        }

        //[HttpPost()]
        //[Route("approved/page")]
        //public GetApprovedPurchaseOderPage.Response Process([FromBody]GetApprovedPurchaseOderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetApprovedPurchaseOderPage.Request());
        //}

        [HttpPost()]
        [Route("{id}/paid")]
        public PayPurchaseOrder.Response Process([FromUri]Guid id, [FromBody]PayPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new PayPurchaseOrder.Request());
        }

        //[HttpPost()]
        //[Route("paid/page")]
        //public GetPaidPurchaseOrderPage.Response Process([FromBody]GetPaidPurchaseOrderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetPaidPurchaseOrderPage.Request());
        //}

        [HttpPost()]
        [Route("{id}/received")]
        public ReceivePurchaseOrder.Response Process([FromUri]Guid id, [FromBody]ReceivePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new ReceivePurchaseOrder.Request());
        }

        [HttpGet()]
        [Route("{id}/receivables")]
        public GetPurchaseOrderReceivableList.Response Process([FromUri]GetPurchaseOrderReceivableList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderReceivableList.Request());
        }

        //[HttpPost()]
        //[Route("received/page")]
        //public GetPaidPurchaseOrderPage.Response Process([FromBody]GetPaidPurchaseOrderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetPaidPurchaseOrderPage.Request());
        //}

        [HttpPost()]
        [Route("{id}/cancelled")]
        public CancelPurchaseOder.Response Process([FromUri]Guid id, [FromBody]CancelPurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CancelPurchaseOder.Request());
        }

        //[HttpPost()]
        //[Route("cancelled/page")]
        //public GetCancelledPurchaseOderPage.Response Process([FromBody]GetCancelledPurchaseOderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetCancelledPurchaseOderPage.Request());
        //}

        [HttpPost()]
        [Route("{id}/completed")]
        public CompletePurchaseOder.Response Process([FromUri]Guid id, [FromBody]CompletePurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CompletePurchaseOder.Request());
        }

        //[HttpPost()]
        //[Route("completed/page")]
        //public GetCompletedPurchaseOderPage.Response Process([FromBody]GetCompletedPurchaseOderPage.Request request)
        //{
        //    return _mediator.Send(request ?? new GetCompletedPurchaseOderPage.Request());
        //}
    }
}
