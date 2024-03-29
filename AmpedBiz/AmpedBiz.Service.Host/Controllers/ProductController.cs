﻿using AmpedBiz.Service.Products;
using MediatR;
using System.Threading.Tasks;
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

        [HttpGet()]
        [Route("{request.id}")]
        public async Task<GetProduct.Response> Process([FromUri]GetProduct.Request request)
        {
            return await _mediator.Send(request ?? new GetProduct.Request());
        }

        [HttpGet()]
        [Route("~/product-inventories/{request.key}")]
        public async Task<GetProductInventory.Response> Process([FromUri]GetProductInventory.Request request)
        {
            return await _mediator.Send(request ?? new GetProductInventory.Request());
        }

		[HttpGet()]
        [Route("~/product-inventories")]
        public async Task<GetProductInventoryList.Response> Process([FromUri]GetProductInventoryList.Request request)
        {
            return await _mediator.Send(request ?? new GetProductInventoryList.Request());
        }

        //[HttpGet()]
        //[Route("~/product-inventories/{productId}")]
        //public async Task<GetProductInventoryOld.Response> Process([FromUri]GetProductInventoryOld.Request request)
        //{
        //    return await _mediator.Send(request ?? new GetProductInventoryOld.Request());
        //}

        //[HttpGet()]
        //[Route("~/product-inventories")]
        //public async Task<GetProductInventoryOldList.Response> Process([FromUri]GetProductInventoryOldList.Request request)
        //{
        //    return await _mediator.Send(request ?? new GetProductInventoryOldList.Request());
        //}

        [HttpGet()]
        [Route("~/product-lookups")]
        public async Task<GetProductLookup.Response> Process([FromUri]GetProductLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetProductLookup.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetProductList.Response> Process([FromUri]GetProductList.Request request)
        {
            return await _mediator.Send(request ?? new GetProductList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetProductPage.Response> Process([FromBody]GetProductPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductPage.Request());
        }

        [HttpPost()]
        [Route("needs-reordering/page")]
        public async Task<GetNeedsReorderingPage.Response> Process([FromBody]GetNeedsReorderingPage.Request request)
        {
            return await _mediator.Send(request ?? new GetNeedsReorderingPage.Request());
        }

        [HttpPost()]
        [Route("discontinued/page")]
        public async Task<GetDiscontinuedPage.Response> Process([FromBody]GetDiscontinuedPage.Request request)
        {
            return await _mediator.Send(request ?? new GetDiscontinuedPage.Request());
        }

        [HttpPost()]
        [Route("inventory-level/page")]
        public async Task<GetProductInventoryLevelPage.Response> Process([FromBody]GetProductInventoryLevelPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductInventoryLevelPage.Request());
        }

        [HttpPost()]
        [Route("{id}/orders/page")]
        public async Task<GetProductOrderPage.Response> Process([FromUri]string id, [FromBody]GetProductOrderPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductOrderPage.Request());
        }

        [HttpPost()]
        [Route("{id}/order-returns/page")]
        public async Task<GetProductOrderReturnPage.Response> Process([FromUri]string id, [FromBody]GetProductOrderReturnPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductOrderReturnPage.Request());
        }

        [HttpPost()]
        [Route("{id}/purchases/page")]
        public async Task<GetProductPurchasePage.Response> Process([FromUri]string id, [FromBody]GetProductPurchasePage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductPurchasePage.Request());
        }

        [HttpPost()]
        [Route("{id}/returns/page")]
        public async Task<GetProductReturnPage.Response> Process([FromUri]string id, [FromBody]GetProductReturnPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductReturnPage.Request());
        }

        [HttpPost()]
        [Route("delivery-report/page")]
        public async Task<GetProductDeliveryReportPage.Response> Process([FromBody]GetProductDeliveryReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductDeliveryReportPage.Request());
        }

        [HttpPost()]
        [Route("listing-report/page")]
        public async Task<GetProductListingReportPage.Response> Process([FromBody]GetProductListingReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductListingReportPage.Request());
        }

        [HttpPost()]
        [Route("retail-price-details/page")]
        public async Task<GetProductRetailPriceDetailsPage.Response> Process([FromBody]GetProductRetailPriceDetailsPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductRetailPriceDetailsPage.Request());
        }

        [HttpPost()]
        [Route("report/page")]
        public async Task<GetProductReportPage.Response> Process([FromBody]GetProductReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductReportPage.Request());
        }

		[HttpPost()]
		[Route("sales-report/page")]
		public async Task<GetProductSalesReportPage.Response> Process([FromBody]GetProductSalesReportPage.Request request)
		{
			return await _mediator.Send(request ?? new GetProductSalesReportPage.Request());
		}

		[HttpPost()]
        [Route("")]
        public async Task<CreateProduct.Response> Process([FromBody]CreateProduct.Request request)
        {
            return await _mediator.Send(request ?? new CreateProduct.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateProduct.Response> Process([FromBody]UpdateProduct.Request request)
        {
            return await _mediator.Send(request ?? new UpdateProduct.Request());
        }
	}
}
