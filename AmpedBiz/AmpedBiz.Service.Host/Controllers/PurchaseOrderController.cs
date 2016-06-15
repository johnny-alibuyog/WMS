using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AmpedBiz.Service.PurchaseOrders;
using MediatR;

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
        public GetPurchaseOrder.Response Get([FromUri]GetPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrder.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetPurchaseOrderList.Response Get([FromUri]GetPurchaseOrderList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetPurchaseOrderPage.Response Page([FromBody]GetPurchaseOrderPage.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreatePurchaseOrder.Response Create([FromBody]CreatePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new CreatePurchaseOrder.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdatePurchaseOrder.Response Update([FromBody]UpdatePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new UpdatePurchaseOrder.Request());
        }

        [HttpGet()]
        [Route("statuses")]
        public GetPurchaseOrderStatusList.Response GetStatuses([FromBody]GetPurchaseOrderStatusList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderStatusList.Request());
        }

        [HttpGet()]
        [Route("statuses/lookup")]
        public GetPurchaseOrderStatusLookup.Response GetStatusLookup([FromBody]GetPurchaseOrderStatusLookup.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOrderStatusLookup.Request());
        }

        [HttpPost()]
        [Route("new/page")]
        public GetNewPurchaseOrdersPage.Response GetNewPage([FromBody]GetNewPurchaseOrdersPage.Request request)
        {
            return _mediator.Send(request ?? new GetNewPurchaseOrdersPage.Request());
        }

        [HttpPost()]
        [Route("active/page")]
        public GetActivePurchaseOrdersPage.Response GetSubmittedPage([FromBody]GetActivePurchaseOrdersPage.Request request)
        {
            return _mediator.Send(request ?? new GetActivePurchaseOrdersPage.Request());
        }

        [HttpPost()]
        [Route("{request.id}/submitted")]
        public SubmitPurchaseOrder.Response CreateSubmitted([FromUri]SubmitPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SubmitPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("submitted/page")]
        public GetSubmittedPurchaseOrdersPage.Response GetSubmittedPage([FromBody]GetSubmittedPurchaseOrdersPage.Request request)
        {
            return _mediator.Send(request ?? new GetSubmittedPurchaseOrdersPage.Request());
        }

        [HttpPost()]
        [Route("{id}/approved")]
        public ApprovePurchaseOrder.Response CreateApproved([FromUri]Guid id, [FromBody]ApprovePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new ApprovePurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("approved/page")]
        public GetApprovedPurchaseOrdersPage.Response GetApprovedPage([FromBody]GetApprovedPurchaseOrdersPage.Request request)
        {
            return _mediator.Send(request ?? new GetApprovedPurchaseOrdersPage.Request());
        }

        [HttpPost()]
        [Route("{id}/cancelled")]
        public CancelPurchaseOrder.Response CreateCancelled([FromUri]Guid id, [FromBody]CancelPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new CancelPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("cancelled/page")]
        public GetCancelledPurchaseOrdersPage.Response GetCancelledPage([FromBody]GetCancelledPurchaseOrdersPage.Request request)
        {
            return _mediator.Send(request ?? new GetCancelledPurchaseOrdersPage.Request());
        }

        [HttpPost()]
        [Route("{id}/completed")]
        public CompletePurchaseOrder.Response CreateCompleted([FromUri]Guid id, [FromBody]CompletePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new CompletePurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("completed/page")]
        public GetCompletedPurchaseOrdersPage.Response GetCompletedPage([FromBody]GetCompletedPurchaseOrdersPage.Request request)
        {
            return _mediator.Send(request ?? new GetCompletedPurchaseOrdersPage.Request());
        }
    }
}
