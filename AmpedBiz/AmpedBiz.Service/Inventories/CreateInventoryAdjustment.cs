using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Inventories;
using AmpedBiz.Data;
using MediatR;
using NHibernate.Linq;

namespace AmpedBiz.Service.Inventories
{
    public class CreateInventoryAdjustment
    {
        public class Request : Dto.InventoryAdjustment, IRequest<Response> { }

        public class Response : Dto.InventoryAdjustment { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Inventory>(message.InventoryId);

                    entity.EnsureExistence($"Inventory with id {message.InventoryId} does not exists.");

                    entity.Accept(new InventoryAdjustVisitor(
                        adjustedBy: message.AdjustedBy != null 
                            ? session.Load<User>(message.AdjustedBy.Id) 
                            : null,
                        adjustedOn: message.AdjustedOn,
                        reason: message.Reason != null 
                            ? session.Load<InventoryAdjustmentReason>(message.Reason.Id) 
                            : null,
                        remarks: message.Remarks,
                        quantity: new Measure(
                            value: message.Quantity?.Value ?? 0M,
                            unit: message.Quantity?.Unit != null 
                                ? session.Load<UnitOfMeasure>(message.Quantity.Unit.Id) 
                                : null
                        ),
                        standard: new Measure(
                            value: message.Standard?.Value ?? 0M,
                            unit: message.Standard?.Unit != null 
                                ? session.Load<UnitOfMeasure>(message.Standard.Unit.Id)
                                : null
                        )
                    ));

                    transaction.Commit();

                    entity.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }
    }
}
