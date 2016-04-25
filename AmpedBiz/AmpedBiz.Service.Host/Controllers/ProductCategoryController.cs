using System.Web.Http;
using AmpedBiz.Service.ProductCategories;
using MediatR;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("product-categories")]
    public class ProductCategoryController : ApiController
    {
        private readonly IMediator _mediator;

        public ProductCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetProductCategory.Response Get([FromUri]GetProductCategory.Request request)
        {
            return _mediator.Send(request ?? new GetProductCategory.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetProductCategories.Response Get([FromUri]GetProductCategories.Request request)
        {
            return _mediator.Send(request ?? new GetProductCategories.Request());
        }

        [HttpPost()]
        [Route("pages")]
        public GetProductCategoryPages.Response Page([FromBody]GetProductCategoryPages.Request request)
        {
            return _mediator.Send(request ?? new GetProductCategoryPages.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateProductCategory.Response Create([FromBody]CreateProductCategory.Request request)
        {
            return _mediator.Send(request ?? new CreateProductCategory.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateProductCategory.Response Update([FromBody]UpdateProductCategory.Request request)
        {
            return _mediator.Send(request ?? new UpdateProductCategory.Request());
        }
    }
}