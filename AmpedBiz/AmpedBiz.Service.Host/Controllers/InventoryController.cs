using AmpedBiz.Service.Inventories;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace AmpedBiz.Service.Host.Controllers
{
    [RoutePrefix("inventories")]
    public class InventoryController : ApiController
    {
        private readonly IMediator _mediator;

        public InventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost()]
        [Route("{id}/adjustments")]
        public async Task<CreateInventoryAdjustment.Response> Process([FromUri]Guid id, [FromBody]CreateInventoryAdjustment.Request request)
        {
            return await _mediator.Send(request ?? new CreateInventoryAdjustment.Request());
        }

        [HttpPost()]
        [Route("{id}/adjustments/page")]
        public async Task<GetInventoryAdjustmentPage.Response> Process([FromUri]Guid id, [FromBody]GetInventoryAdjustmentPage.Request request)
        {
            return await _mediator.Send(request ?? new GetInventoryAdjustmentPage.Request());
        }

        [HttpPost()]
        [Route("movements-report/page")]
        public async Task<GetInventoryMovementsReportPage.Response> Process([FromBody]GetInventoryMovementsReportPage.Request request)
        {
            return await _mediator.Send(request ?? new GetInventoryMovementsReportPage.Request());
        }

        [HttpGet()]
        [Route("adjustment-reasons")]
        public async Task<GetInventoryAdjustmentReasonList.Response> Process([FromUri]GetInventoryAdjustmentReasonList.Request request)
        {
            return await _mediator.Send(request ?? new GetInventoryAdjustmentReasonList.Request());
        }

        [HttpGet()]
        [Route("adjustment-types")]
        public async Task<GetInventoryAdjustmentTypeList.Response> Process([FromUri]GetInventoryAdjustmentTypeList.Request request)
        {
            return await _mediator.Send(request ?? new GetInventoryAdjustmentTypeList.Request());
        }

        [HttpGet()]
        [Route("adjustment-type-lookup")]
        public async Task<GetInventoryAdjustmentTypeLookup.Response> Process([FromUri]GetInventoryAdjustmentTypeLookup.Request request)
        {
            return await _mediator.Send(request ?? new GetInventoryAdjustmentTypeLookup.Request());
        }
    }
}