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
        public GetSuppliers.Response Get([FromUri]GetSuppliers.Request request)
        {
            return _mediator.Send(request ?? new GetSuppliers.Request());
        }

        [HttpPost()]
        [Route("pages")]
        public GetSupplierPages.Response Page([FromBody]GetSupplierPages.Request request)
        {
            return _mediator.Send(request ?? new GetSupplierPages.Request());
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