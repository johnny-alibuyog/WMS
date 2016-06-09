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
    [RoutePrefix("purchaseorders")]
    public class PurchaseOrderController : ApiController
    {
        private readonly IMediator _mediator;

        public PurchaseOrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public PurchaseOrderController()
        {
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

        [HttpPost()]
        [Route("submit")]
        public SubmitPurchaseOrder.Response Submit([FromBody]SubmitPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new SubmitPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("approve")]
        public ApprovePurchaseOrder.Response Approve([FromBody]ApprovePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new ApprovePurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("reject")]
        public RejectPurchaseOrder.Response Reject([FromBody]RejectPurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new RejectPurchaseOrder.Request());
        }

        [HttpPost()]
        [Route("complete")]
        public CompletePurchaseOrder.Response Complete([FromBody]CompletePurchaseOrder.Request request)
        {
            return _mediator.Send(request ?? new CompletePurchaseOrder.Request());
        }
    }
}
