using AmpedBiz.Service.Orders;
using MediatR;
using System;
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
        public GetOrder.Response Process([FromUri]GetOrder.Request request)
        {
            return _mediator.Send(request ?? new GetOrder.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetOrderList.Response Process([FromUri]GetOrderList.Request request)
        {
            return _mediator.Send(request ?? new GetOrderList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetOrderPage.Response Process([FromBody]GetOrderPage.Request request)
        {
            return _mediator.Send(request ?? new GetOrderPage.Request());
        }

        [HttpGet()]
        [Route("statuses")]
        public GetOrderStatusList.Response Process([FromBody]GetOrderStatusList.Request request)
        {
            return _mediator.Send(request ?? new GetOrderStatusList.Request());
        }

        [HttpGet()]
        [Route("status-lookups")]
        public GetOrderStatusLookup.Response Process([FromBody]GetOrderStatusLookup.Request request)
        {
            return _mediator.Send(request ?? new GetOrderStatusLookup.Request());
        }

        //[HttpPost()]
        //[Route("new")]
        //public CreateNewOder.Response Process([FromBody]CreateNewOder.Request request)
        //{
        //    return _mediator.Send(request ?? new CreateNewOder.Request());
        //}

        //[HttpPatch()]
        //[Route("{id}/new")]
        //public UpdateNewOder.Response Process([FromUri]Guid id, [FromBody]UpdateNewOder.Request request)
        //{
        //    return _mediator.Send(request ?? new UpdateNewOder.Request());
        //}

        ////[HttpPost()]
        ////[Route("new/page")]
        ////public GetNewOderPage.Response Process([FromBody]GetNewOderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetNewOderPage.Request());
        ////}

        ////[HttpPost()]
        ////[Route("active/page")]
        ////public GetActiveOderPage.Response Process([FromBody]GetActiveOderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetActiveOderPage.Request());
        ////}

        //[HttpPost()]
        //[Route("{id}/submitted")]
        //public SubmitOrder.Response Process([FromUri]Guid id, [FromBody]SubmitOrder.Request request)
        //{
        //    return _mediator.Send(request ?? new SubmitOrder.Request());
        //}

        ////[HttpPost()]
        ////[Route("submitted/page")]
        ////public GetSubmittedOderPage.Response Process([FromBody]GetSubmittedOderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetSubmittedOderPage.Request());
        ////}

        //[HttpPost()]
        //[Route("{id}/approved")]
        //public ApproveOder.Response Process([FromUri]Guid id, [FromBody]ApproveOder.Request request)
        //{
        //    return _mediator.Send(request ?? new ApproveOder.Request());
        //}

        ////[HttpPost()]
        ////[Route("approved/page")]
        ////public GetApprovedOderPage.Response Process([FromBody]GetApprovedOderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetApprovedOderPage.Request());
        ////}

        //[HttpPost()]
        //[Route("{id}/paid")]
        //public PayOrder.Response Process([FromUri]Guid id, [FromBody]PayOrder.Request request)
        //{
        //    return _mediator.Send(request ?? new PayOrder.Request());
        //}

        ////[HttpPost()]
        ////[Route("paid/page")]
        ////public GetPaidOrderPage.Response Process([FromBody]GetPaidOrderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetPaidOrderPage.Request());
        ////}

        //[HttpPost()]
        //[Route("{id}/received")]
        //public ReceiveOrder.Response Process([FromUri]Guid id, [FromBody]ReceiveOrder.Request request)
        //{
        //    return _mediator.Send(request ?? new ReceiveOrder.Request());
        //}

        //[HttpGet()]
        //[Route("{id}/receivables")]
        //public GetOrderReceivableList.Response Process([FromUri]GetOrderReceivableList.Request request)
        //{
        //    return _mediator.Send(request ?? new GetOrderReceivableList.Request());
        //}

        ////[HttpPost()]
        ////[Route("received/page")]
        ////public GetPaidOrderPage.Response Process([FromBody]GetPaidOrderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetPaidOrderPage.Request());
        ////}

        //[HttpPost()]
        //[Route("{id}/cancelled")]
        //public CancelOder.Response Process([FromUri]Guid id, [FromBody]CancelOder.Request request)
        //{
        //    return _mediator.Send(request ?? new CancelOder.Request());
        //}

        ////[HttpPost()]
        ////[Route("cancelled/page")]
        ////public GetCancelledOderPage.Response Process([FromBody]GetCancelledOderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetCancelledOderPage.Request());
        ////}

        //[HttpPost()]
        //[Route("{id}/completed")]
        //public CompleteOder.Response Process([FromUri]Guid id, [FromBody]CompleteOder.Request request)
        //{
        //    return _mediator.Send(request ?? new CompleteOder.Request());
        //}

        ////[HttpPost()]
        ////[Route("completed/page")]
        ////public GetCompletedOderPage.Response Process([FromBody]GetCompletedOderPage.Request request)
        ////{
        ////    return _mediator.Send(request ?? new GetCompletedOderPage.Request());
        ////}
    }
}