using System.Web.Http;
using AmpedBiz.Service.UnitOfMeasures;
using MediatR;


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
        public GetUnitOfMeasure.Response Get([FromUri]GetUnitOfMeasure.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasure.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetUnitOfMeasureList.Response Get([FromUri]GetUnitOfMeasureList.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasureList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetUnitOfMeasurePage.Response Page([FromBody]GetUnitOfMeasurePage.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasurePage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateUnitOfMeasure.Response Create([FromBody]CreateUnitOfMeasure.Request request)
        {
            return _mediator.Send(request ?? new CreateUnitOfMeasure.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateUnitOfMeasure.Response Update([FromBody]UpdateUnitOfMeasure.Request request)
        {
            return _mediator.Send(request ?? new UpdateUnitOfMeasure.Request());
        }
    }
}