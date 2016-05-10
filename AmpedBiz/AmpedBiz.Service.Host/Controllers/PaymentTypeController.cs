﻿using System.Web.Http;
using AmpedBiz.Service.PaymentTypes;
using MediatR;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("payment-types")]
    public class PaymentTypeController : ApiController
    {
        private readonly IMediator _mediator;

        public PaymentTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetPaymentType.Response Get([FromUri]GetPaymentType.Request request)
        {
            return _mediator.Send(request ?? new GetPaymentType.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetPaymentTypeList.Response Get([FromUri]GetPaymentTypeList.Request request)
        {
            return _mediator.Send(request ?? new GetPaymentTypeList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetPaymentTypePage.Response Page([FromBody]GetPaymentTypePage.Request request)
        {
            return _mediator.Send(request ?? new GetPaymentTypePage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreatePaymentType.Response Create([FromBody]CreatePaymentType.Request request)
        {
            return _mediator.Send(request ?? new CreatePaymentType.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdatePaymentType.Response Update([FromBody]UpdatePaymentType.Request request)
        {
            return _mediator.Send(request ?? new UpdatePaymentType.Request());
        }
    }
}
