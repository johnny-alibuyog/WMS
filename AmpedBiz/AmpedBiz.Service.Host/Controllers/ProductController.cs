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
        public GetProduct.Response Process([FromUri]GetProduct.Request request)
        {
            return _mediator.Send(request ?? new GetProduct.Request());
        }


        [HttpGet()]
        [Route("~/product-inventories/{productId}")]
        public GetProductInventory.Response Process([FromUri]GetProductInventory.Request request)
        {
            return _mediator.Send(request ?? new GetProductInventory.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetProductList.Response Process([FromUri]GetProductList.Request request)
        {
            return _mediator.Send(request ?? new GetProductList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetProductPage.Response Process([FromBody]GetProductPage.Request request)
        {
            return _mediator.Send(request ?? new GetProductPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateProduct.Response Process([FromBody]CreateProduct.Request request)
        {
            return _mediator.Send(request ?? new CreateProduct.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateProduct.Response Process([FromBody]UpdateProduct.Request request)
        {
            return _mediator.Send(request ?? new UpdateProduct.Request());
        }
    }
}
