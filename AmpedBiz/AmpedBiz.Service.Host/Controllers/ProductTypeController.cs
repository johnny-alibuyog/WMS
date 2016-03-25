using System.Web.Http;
using AmpedBiz.Service.ProductTypes;
using MediatR;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("product-types")]
    public class ProductTypeController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetProductType.Response Get([FromUri]GetProductType.Request request)
        {
            return _mediator.Send(request ?? new GetProductType.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetProductTypes.Response Get([FromUri]GetProductTypes.Request request)
        {
            return _mediator.Send(request ?? new GetProductTypes.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateProductType.Response Create([FromBody]CreateProductType.Request request)
        {
            return _mediator.Send(request ?? new CreateProductType.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateProductType.Response Update([FromBody]UpdateProductType.Request request)
        {
            return _mediator.Send(request ?? new UpdateProductType.Request());
        }
    }
}
