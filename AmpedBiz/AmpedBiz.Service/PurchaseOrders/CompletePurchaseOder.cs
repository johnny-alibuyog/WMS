using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.PurchaseOrders;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CompletePurchaseOder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            private void Hydrate(Response response)
            {
                var handler = new GetPurchaseOrder.Handler(this._sessionFactory, this._context);
                var hydrated = handler.Handle(new GetPurchaseOrder.Request(response.Id));

                hydrated.MapTo(response);
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    entity.EnsureExistence($"PurchaseOrder with id {message.Id} does not exists.");
                    entity.State.Process(new PurchaseOrderCompletedVisitor()
                    { 
                        CompletedBy = session.Load<User>(message.CompletedBy.Id),
                        CompletedOn = message.CompletedOn ?? DateTime.Now
                    });
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    response.Id = entity.Id;
                    //entity.MapTo(response);
                }

                Hydrate(response);

                return response;
            }
        }
    }
}