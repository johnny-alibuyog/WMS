﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using AmpedBiz.Data;
using MediatR;
using System;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Orders
{
    public class RecreateOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public override Response Execute(Request request)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<Order>(request.Id);
                    entity.EnsureExistence($"Order with id {request.Id} does not exists.");
                    entity.State.Process(new OrderModifiedBackVisitor()
                    {
                        Branch = session.Load<Branch>(this.Context.BranchId),
                        RecreatedOn = request.RecreatedOn ?? DateTime.Today,
                        RecreatedBy = session.Load<User>(request.RecreatedBy.Id)
                    });
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    response.Id = entity.Id;
                    //entity.MapTo(response);

                    SessionFactory.ReleaseSharedSession();
                }

                return response;
            }
        }

        public class PostProcess : RequestPostProcessorBase<Request, Response>
        {
            public override Task Execute(Request request, Response response)
            {
                // hydrate the response with the new object state

                var hydrationHandler = new GetOrder.Handler()
                {
                    SessionFactory = this.sessionFactory,
                    Context = this.context
                };

                var hydrated = hydrationHandler.Execute(new GetOrder.Request(response.Id));

                response.MapFrom(hydrated);

                return Task.FromResult(0);
            }
        }
    }
}