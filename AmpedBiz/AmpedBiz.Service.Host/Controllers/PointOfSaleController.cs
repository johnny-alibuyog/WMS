using AmpedBiz.Service.PointOfSales;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
	[RoutePrefix("point-of-sales")]
	public class PointOfSaleController : ApiController
	{
		private readonly IMediator _mediator;

		public PointOfSaleController(IMediator mediator)
		{
			this._mediator = mediator;
		}

		[HttpGet()]
		[Route("{request.id}")]
		public async Task<GetPointOfSale.Response> Process([FromUri]GetPointOfSale.Request request)
		{
			return await _mediator.Send(request ?? new GetPointOfSale.Request());
		}

		[HttpPost()]
		[Route("")]
		public async Task<SavePointOfSale.Response> Process([FromBody]SavePointOfSale.Request request)
		{
			return await _mediator.Send(request ?? new SavePointOfSale.Request());
		}

		[HttpPost()]
		[Route("page")]
		public async Task<GetPointOfSalePage.Response> Process([FromBody]GetPointOfSalePage.Request request)
		{
			return await _mediator.Send(request ?? new GetPointOfSalePage.Request());
		}

		[HttpGet()]
		[Route("statuses")]
		public async Task<GetPointOfSaleStatusList.Response> Process([FromBody]GetPointOfSaleStatusList.Request request)
		{
			return await _mediator.Send(request ?? new GetPointOfSaleStatusList.Request());
		}

		[HttpGet()]
		[Route("status-lookups")]
		public async Task<GetPointOfSaleStatusLookup.Response> Process([FromUri]GetPointOfSaleStatusLookup.Request request)
		{
			return await _mediator.Send(request ?? new GetPointOfSaleStatusLookup.Request());
		}
	}
}