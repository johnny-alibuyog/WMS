using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Envents.PurchaseOrders;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CancelPurchaseOder
    {
        public class Request : Dto.PurchaseOrderCancelledEvent, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    var cancelledEvent = new PurchaseOrderCancelledEvent(
                        cancelledBy: session.Load<User>(message.CancelledBy.Id),
                        cancelledOn: message.CancelledOn ?? DateTime.Now,
                        cancellationReason: message.CancellationReason
                    );

                    entity.State.Cancel(cancelledEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}