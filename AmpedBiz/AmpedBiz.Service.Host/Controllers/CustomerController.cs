using AmpedBiz.Service.Customers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("customers")]
    public class CustomerController : ApiController
    {
        private readonly IMediator _mediator;

        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetCustomer.Response Get([FromUri]GetCustomer.Request request)
        {
            return _mediator.Send(request ?? new GetCustomer.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetCustomerList.Response Get([FromUri]GetCustomerList.Request request)
        {
            return _mediator.Send(request ?? new GetCustomerList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetCustomerPage.Response Page([FromBody]GetCustomerPage.Request request)
        {
            return _mediator.Send(request ?? new GetCustomerPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateCustomer.Response Create([FromBody]CreateCustomer.Request request)
        {
            return _mediator.Send(request ?? new CreateCustomer.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateCustomer.Response Update([FromBody]UpdateCustomer.Request request)
        {
            return _mediator.Send(request ?? new UpdateCustomer.Request());
        }
    }
}
