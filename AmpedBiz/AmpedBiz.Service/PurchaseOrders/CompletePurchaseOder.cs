using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.PurchaseOrders;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Threading.Tasks;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CompletePurchaseOder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request request)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<PurchaseOrder>(request.Id);
                    entity.EnsureExistence($"PurchaseOrder with id {request.Id} does not exists.");
                    entity.State.Process(new PurchaseOrderCompletedVisitor()
                    { 
                        Branch = session.Load<Branch>(Context.BranchId),
                        CompletedBy = session.Load<User>(request.CompletedBy.Id),
                        CompletedOn = request.CompletedOn ?? DateTime.Now
                    });
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    response.Id = entity.Id;
                    //entity.MapTo(response);
                }

                return response;
            }
        }

        public class PostProcess : RequestPostProcessorBase<Request, Response>
        {
            public override Task Execute(Request request, Response response)
            {
                // hydrate the response with the new object state

                var hydrationHandler = new GetPurchaseOrder.Handler()
                {
                    SessionFactory = this.sessionFactory,
                    Context = this.context
                };

                var hydrated = hydrationHandler.Execute(new GetPurchaseOrder.Request(response.Id));

                response.MapFrom(hydrated);

                return Task.FromResult(0);
            }
        }
    }
}