using AmpedBiz.Service.UnitOfMeasureClasses;
using MediatR;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("unit-of-measure-classes")]
    public class UnitOfMeasureClassController : ApiController
    {
        private readonly IMediator _mediator;

        public UnitOfMeasureClassController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        [Route("{request.id}")]
        public GetUnitOfMeasureClass.Response Process([FromUri]GetUnitOfMeasureClass.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasureClass.Request());
        }

        [HttpGet()]
        [Route("")]
        public GetUnitOfMeasureClassList.Response Process([FromUri]GetUnitOfMeasureClassList.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasureClassList.Request());
        }

        [HttpPost()]
        [Route("page")]
        public GetUnitOfMeasureClassPage.Response Process([FromBody]GetUnitOfMeasureClassPage.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasureClassPage.Request());
        }

        [HttpGet()]
        [Route("~/unit-of-measure-class-lookups")]
        public GetUnitOfMeasureLookup.Response Process([FromUri]GetUnitOfMeasureLookup.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasureLookup.Request());
        }

        [HttpPost()]
        [Route("report/page")]
        public GetUnitOfMeasureClassReportPage.Response Process([FromBody]GetUnitOfMeasureClassReportPage.Request request)
        {
            return _mediator.Send(request ?? new GetUnitOfMeasureClassReportPage.Request());
        }

        [HttpPost()]
        [Route("")]
        public CreateUnitOfMeasureClass.Response Process([FromBody]CreateUnitOfMeasureClass.Request request)
        {
            return _mediator.Send(request ?? new CreateUnitOfMeasureClass.Request());
        }

        [HttpPut()]
        [Route("")]
        public UpdateUnitOfMeasureClass.Response Process([FromBody]UpdateUnitOfMeasureClass.Request request)
        {
            return _mediator.Send(request ?? new UpdateUnitOfMeasureClass.Request());
        }
    }
}