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
        public GetProductCategory.Response Process([FromUri]GetProductCategory.Request request)
        {
            return _mediator.Send(request ?? new GetProductCategory.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetProductCategoryList.Response Process([FromUri]GetProductCategoryList.Request request)
        {
            return _mediator.Send(request ?? new GetProductCategoryList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetProductCategoryPage.Response Process([FromBody]GetProductCategoryPage.Request request)
        {
            return _mediator.Send(request ?? new GetProductCategoryPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateProductCategory.Response Process([FromBody]CreateProductCategory.Request request)
        {
            return _mediator.Send(request ?? new CreateProductCategory.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateProductCategory.Response Process([FromBody]UpdateProductCategory.Request request)
        {
            return _mediator.Send(request ?? new UpdateProductCategory.Request());
        }
    }
}