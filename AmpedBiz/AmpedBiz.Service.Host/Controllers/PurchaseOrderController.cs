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
        public GetPurchaseOder.Response Process([FromUri]GetPurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOder.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetPurchaseOderList.Response Process([FromUri]GetPurchaseOderList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOderList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetPurchaseOderPage.Response Process([FromBody]GetPurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOderPage.Request());
        }

        [HttpGet()]
        [Route("statuses")]
        public GetPurchaseOderStatusList.Response Process([FromBody]GetPurchaseOderStatusList.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOderStatusList.Request());
        }

        [HttpGet()]
        [Route("status-lookups")]
        public GetPurchaseOderStatusLookup.Response Process([FromBody]GetPurchaseOderStatusLookup.Request request)
        {
            return _mediator.Send(request ?? new GetPurchaseOderStatusLookup.Request());
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

        [HttpPost()]
        [Route("new/page")]
        public GetNewPurchaseOderPage.Response Process([FromBody]GetNewPurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetNewPurchaseOderPage.Request());
        }

        [HttpPost()]
        [Route("active/page")]
        public GetActivePurchaseOderPage.Response Process([FromBody]GetActivePurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetActivePurchaseOderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/submitted")]
        public SubmitPurchaseOrder.Response Process([FromUri]Guid id, [FromBody]SubmitPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SubmitPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("submitted/page")]
        public GetSubmittedPurchaseOderPage.Response Process([FromBody]GetSubmittedPurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetSubmittedPurchaseOderPage.Request());
        }

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
        [Route("{id}/approved")]
        public ApprovePurchaseOder.Response Process([FromUri]Guid id, [FromBody]ApprovePurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new ApprovePurchaseOder.Request());
        }

        [HttpPost()]
        [Route("approved/page")]
        public GetApprovedPurchaseOderPage.Response Process([FromBody]GetApprovedPurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetApprovedPurchaseOderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/cancelled")]
        public CancelPurchaseOder.Response Process([FromUri]Guid id, [FromBody]CancelPurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CancelPurchaseOder.Request());
        }

        [HttpPost()]
        [Route("cancelled/page")]
        public GetCancelledPurchaseOderPage.Response Process([FromBody]GetCancelledPurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetCancelledPurchaseOderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/completed")]
        public CompletePurchaseOder.Response Process([FromUri]Guid id, [FromBody]CompletePurchaseOder.Request request)
        {
            return _mediator.Send(request ?? new CompletePurchaseOder.Request());
        }

        [HttpPost()]
        [Route("completed/page")]
        public GetCompletedPurchaseOderPage.Response Process([FromBody]GetCompletedPurchaseOderPage.Request request)
        {
            return _mediator.Send(request ?? new GetCompletedPurchaseOderPage.Request());
        }
    }
}
