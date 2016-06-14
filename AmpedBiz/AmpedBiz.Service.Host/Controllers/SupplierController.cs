using System.Web.Http;
using AmpedBiz.Service.Suppliers;
using MediatR;

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
        public GetSupplier.Response Get([FromUri]GetSupplier.Request request)
        {
            return _mediator.Send(request ?? new GetSupplier.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetSupplierList.Response Get([FromUri]GetSupplierList.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetSupplierPage.Response Page([FromBody]GetSupplierPage.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierPage.Request());
        }

        [HttpGet()]
        [Route("lookup")]
        public GetSupplierLookup.Response GetStatus([FromBody]GetSupplierLookup.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateSupplier.Response Create([FromBody]CreateSupplier.Request request)
        {
            return _mediator.Send(request ?? new CreateSupplier.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateSupplier.Response Update([FromBody]UpdateSupplier.Request request)
        {
            return _mediator.Send(request ?? new UpdateSupplier.Request());
        }
    }
}