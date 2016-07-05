using System.Web.Http;
using AmpedBiz.Service.Suppliers;
using MediatR;
using AmpedBiz.Service.Products;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("suppliers")]
    public class SupplierController : ApiController
    {
        private readonly IMediator _mediator;

        public SupplierController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetSupplier.Response Process([FromUri]GetSupplier.Request request)
        {
            return _mediator.Send(request ?? new GetSupplier.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetSupplierList.Response Process([FromUri]GetSupplierList.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierList.Request());
        }

        [HttpGet()]
        [Route("{supplierId}/product-inventories")]
        public GetProductInventoryList.Response Process([FromUri]GetProductInventoryList.Request request)
        {
            return _mediator.Send(request ?? new GetProductInventoryList.Request());
        }

        [HttpGet()]
        [Route("{supplierId}/product-lookups")]
        public GetProductLookup.Response Process([FromUri]GetProductLookup.Request request)
        {
            return _mediator.Send(request ?? new GetProductLookup.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetSupplierPage.Response Process([FromBody]GetSupplierPage.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierPage.Request());
        }

        [HttpGet()]
        [Route("~/supplier-lookups")]
        public GetSupplierLookup.Response Process([FromBody]GetSupplierLookup.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateSupplier.Response Process([FromBody]CreateSupplier.Request request)
        {
            return _mediator.Send(request ?? new CreateSupplier.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateSupplier.Response Process([FromBody]UpdateSupplier.Request request)
        {
            return _mediator.Send(request ?? new UpdateSupplier.Request());
        }
    }
}