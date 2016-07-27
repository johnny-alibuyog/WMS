﻿using AmpedBiz.Service.Orders;
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

        [HttpPost()]
        [Route("new")]
        public CreateNewOrder.Response Process([FromBody]CreateNewOrder.Request request)
        {
            return _mediator.Send(request ?? new CreateNewOrder.Request());
        }

        [HttpPatch()]
        [Route("{id}/new")]
        public UpdateNewOrder.Response Process([FromUri]Guid id, [FromBody]UpdateNewOrder.Request request)
        {
            return _mediator.Send(request ?? new UpdateNewOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/staged")]
        public StageOrder.Response Process([FromUri]Guid id, [FromBody]StageOrder.Request request)
        {
            return _mediator.Send(request ?? new StageOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/routed")]
        public RouteOrder.Response Process([FromUri]Guid id, [FromBody]RouteOrder.Request request)
        {
            return _mediator.Send(request ?? new RouteOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/invoiced")]
        public InvoiceOrder.Response Process([FromUri]Guid id, [FromBody]InvoiceOrder.Request request)
        {
            return _mediator.Send(request ?? new InvoiceOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/paid")]
        public PayOrder.Response Process([FromUri]Guid id, [FromBody]PayOrder.Request request)
        {
            return _mediator.Send(request ?? new PayOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/shipped")]
        public ShipOrder.Response Process([FromUri]Guid id, [FromBody]ShipOrder.Request request)
        {
            return _mediator.Send(request ?? new ShipOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/completed")]
        public CompleteOrder.Response Process([FromUri]Guid id, [FromBody]CompleteOrder.Request request)
        {
            return _mediator.Send(request ?? new CompleteOrder.Request());
        }

        [HttpPost()]
        [Route("{id}/cancelled")]
        public CancelOrder.Response Process([FromUri]Guid id, [FromBody]CancelOrder.Request request)
        {
            return _mediator.Send(request ?? new CancelOrder.Request());
        }
    }
}