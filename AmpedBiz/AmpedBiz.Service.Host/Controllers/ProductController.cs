using AmpedBiz.Service.Products;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("products")]
    public class ProductController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public ProductController()
        {
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetProduct.Response Get([FromUri]GetProduct.Request request)
        {
            return _mediator.Send(request ?? new GetProduct.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetProductList.Response Get([FromUri]GetProductList.Request request)
        {
            return _mediator.Send(request ?? new GetProductList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetProductPage.Response Page([FromBody]GetProductPage.Request request)
        {
            return _mediator.Send(request ?? new GetProductPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateProduct.Response Create([FromBody]CreateProduct.Request request)
        {
            return _mediator.Send(request ?? new CreateProduct.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateProduct.Response Update([FromBody]UpdateProduct.Request request)
        {
            return _mediator.Send(request ?? new UpdateProduct.Request());
        }
    }
}
