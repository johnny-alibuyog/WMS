using AmpedBiz.Service.UnitOfMeasures;
using MediatR;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("unit-of-measures")]
    public class UnitOfMeasureController : ApiController
    {
        private readonly IMediator _mediator;

        public UnitOfMeasureController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public async Task<GetUnitOfMeasure.Response> Process([FromUri]GetUnitOfMeasure.Request request)
        {
            return await _mediator.Send(request ?? new GetUnitOfMeasure.Request());
        }

        [HttpGet()]
        [Route("")]
        public async Task<GetUnitOfMeasureList.Response> Process([FromUri]GetUnitOfMeasureList.Request request)
        {
            return await _mediator.Send(request ?? new GetUnitOfMeasureList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public async Task<GetUnitOfMeasurePage.Response> Process([FromBody]GetUnitOfMeasurePage.Request request)
        {
            return await _mediator.Send(request ?? new GetUnitOfMeasurePage.Request());
        }

        [HttpGet()]
        [Route("~/unit-of-measure-lookups")]
        public async Task<GetUnitOfMeasureLookup.Response> Process([FromUri]GetUnitOfMeasureLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetUnitOfMeasureLookup.Request());
        }

        [HttpPost()]
        [Route("")]
        public async Task<CreateUnitOfMeasure.Response> Process([FromBody]CreateUnitOfMeasure.Request request)
        {
            return await _mediator.Send(request ?? new CreateUnitOfMeasure.Request());
        }

        [HttpPut()]
        [Route("")]
        public async Task<UpdateUnitOfMeasure.Response> Process([FromBody]UpdateUnitOfMeasure.Request request)
        {
            return await _mediator.Send(request ?? new UpdateUnitOfMeasure.Request());
        }
    }
}