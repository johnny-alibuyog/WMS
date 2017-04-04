using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using AmpedBiz.Data;
using MediatR;
using NHibernate;
using System;

namespace AmpedBiz.Service.Orders
{
    public class InvoiceOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            private void Hydrate(Response response)
            {
                var handler = new GetOrder.Handler(this._sessionFactory);
                var hydrated = handler.Handle(new GetOrder.Request(response.Id));

                hydrated.MapTo(response);
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var entity = session.Get<Order>(message.Id);
                    entity.EnsureExistence($"Order with id {message.Id} does not exists.");
                    entity.State.Process(new OrderInvoicedVisitor()
                    {
                        InvoicedOn = message.InvoicedOn ?? DateTime.Now,
                        InvoicedBy = session.Load<User>(message.InvoicedBy.Id)
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
