using AmpedBiz.Service.Products;
using AmpedBiz.Service.Suppliers;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task<GetSupplier.Response> Process([FromUri]GetSupplier.Request request)
        {
            return await _mediator.Send(request ?? new GetSupplier.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetSupplierList.Response> Process([FromUri]GetSupplierList.Request request)
        {
            return await _mediator.Send(request ?? new GetSupplierList.Request());
        }

        [HttpGet()]
        [Route("{supplierId}/product-inventories")]
        public async Task<GetProductInventoryOldList.Response> Process([FromUri]GetProductInventoryOldList.Request request)
        {
            return await _mediator.Send(request ?? new GetProductInventoryOldList.Request());
        }

        [HttpGet()]
        [Route("{supplierId}/product-lookups")]
        public async Task<GetProductLookup.Response> Process([FromUri]GetProductLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetProductLookup.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetSupplierPage.Response> Process([FromBody]GetSupplierPage.Request request)
        {
            return await _mediator.Send(request ?? new GetSupplierPage.Request());
        }

        [HttpPost()]
        [Route("report/page")]
        public async Task<GetSupplierReportPage.Response> Process([FromBody]GetSupplierReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetSupplierReportPage.Request());
        }

        [HttpGet()]
        [Route("~/supplier-lookups")]
        public async Task<GetSupplierLookup.Response> Process([FromBody]GetSupplierLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetSupplierLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateSupplier.Response> Process([FromBody]CreateSupplier.Request request)
        {
            return await _mediator.Send(request ?? new CreateSupplier.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateSupplier.Response> Process([FromBody]UpdateSupplier.Request request)
        {
            return await _mediator.Send(request ?? new UpdateSupplier.Request());
        }
    }
}