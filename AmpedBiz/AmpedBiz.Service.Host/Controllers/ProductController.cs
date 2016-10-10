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
        [Route("~/product-inventories")]
        public GetProductInventoryList.Response Process([FromUri]GetProductInventoryList.Request request)
        {
            return _mediator.Send(request ?? new GetProductInventoryList.Request());
        }

        [HttpGet()]
        [Route("~/product-lookups")]
        public GetProductLookup.Response Process([FromUri]GetProductLookup.Request request)
        {
            return _mediator.Send(request ?? new GetProductLookup.Request());
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
        [Route("needs-reordering/page")]
        public GetNeedsReorderingPage.Response Process([FromBody]GetNeedsReorderingPage.Request request)
        {
            return _mediator.Send(request ?? new GetNeedsReorderingPage.Request());
        }

        [HttpPost()]
        [Route("discontinued/page")]
        public GetDiscontinuedPage.Response Process([FromBody]GetDiscontinuedPage.Request request)
        {
            return _mediator.Send(request ?? new GetDiscontinuedPage.Request());
        }

        [HttpPost()]
        [Route("inventory-level/page")]
        public GetProductInventoryLevelPage.Response Process([FromBody]GetProductInventoryLevelPage.Request request)
        {
            return _mediator.Send(request ?? new GetProductInventoryLevelPage.Request());
        }

        [HttpPost()]
        [Route("{id}/orders/page")]
        public GetProductOrderPage.Response Process([FromUri]string id, [FromBody]GetProductOrderPage.Request request)
        {
            return _mediator.Send(request ?? new GetProductOrderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/purchases/page")]
        public GetProductPurchasePage.Response Process([FromUri]string id, [FromBody]GetProductPurchasePage.Request request)
        {
            return _mediator.Send(request ?? new GetProductPurchasePage.Request());
        }

        [HttpPost()]
        [Route("report/page")]
        public GetProductReportPage.Response Process([FromBody]GetProductReportPage.Request request)
        {
            return _mediator.Send(request ?? new GetProductReportPage.Request());
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
