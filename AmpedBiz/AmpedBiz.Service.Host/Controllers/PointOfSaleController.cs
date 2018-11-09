using AmpedBiz.Service.PointOfSales;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
	[RoutePrefix("point-of-sales")]
	public class PointOfSaleController
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
		public async Task<SavePointOfSale.Response> Process([FromUri]SavePointOfSale.Request request)
		{
			return await _mediator.Send(request ?? new SavePointOfSale.Request());
		}

		[HttpPost()]
		[Route("page")]
		public async Task<GetPointOfSalePage.Response> Process([FromBody]GetPointOfSalePage.Request request)
		{
			return await _mediator.Send(request ?? new GetPointOfSalePage.Request());
		}
	}
}