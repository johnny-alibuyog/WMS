using AmpedBiz.Service.ProductCategories;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

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
        public async Task<GetProductCategory.Response> Process([FromUri]GetProductCategory.Request request)
        {
            return await _mediator.Send(request ?? new GetProductCategory.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetProductCategoryList.Response> Process([FromUri]GetProductCategoryList.Request request)
        {
            return await _mediator.Send(request ?? new GetProductCategoryList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetProductCategoryPage.Response> Process([FromBody]GetProductCategoryPage.Request request)
        {
            return await _mediator.Send(request ?? new GetProductCategoryPage.Request());
        }

        [HttpGet()]
        [Route("~/product-category-lookups")]
        public async Task<GetProductCategoryLookup.Response> Process([FromBody]GetProductCategoryLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetProductCategoryLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateProductCategory.Response> Process([FromBody]CreateProductCategory.Request request)
        {
            return await _mediator.Send(request ?? new CreateProductCategory.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateProductCategory.Response> Process([FromBody]UpdateProductCategory.Request request)
        {
            return await _mediator.Send(request ?? new UpdateProductCategory.Request());
        }
    }
}