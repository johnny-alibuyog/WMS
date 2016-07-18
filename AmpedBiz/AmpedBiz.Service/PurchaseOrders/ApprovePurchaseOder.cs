using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Envents.PurchaseOrders;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class ApprovePurchaseOder
    {
        public class Request : Dto.PurchaseOrderApprovedEvent, IRequest<Response> { }

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

                    var approvedEvent = new PurchaseOrderApprovedEvent(
                        approvedBy: session.Load<User>(message.ApprovedBy.Id),
                        approvedOn: message.ApprovedOn ?? DateTime.Now
                    );

                    entity.State.Approve(approvedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}