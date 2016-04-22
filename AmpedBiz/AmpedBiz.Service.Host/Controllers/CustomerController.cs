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
        public GetCustomers.Response Get([FromUri]GetCustomers.Request request)
        {
            return _mediator.Send(request ?? new GetCustomers.Request());
        }

        [HttpPost()]
        [Route("pages")]
        public GetCustomerPages.Response Page([FromBody]GetCustomerPages.Request request)
        {
            return _mediator.Send(request ?? new GetCustomerPages.Request());
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
