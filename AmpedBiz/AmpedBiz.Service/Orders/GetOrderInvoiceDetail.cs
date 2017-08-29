using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetOrderInvoiceDetail
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : Dto.OrderInvoiceDetail { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<Order>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Items.First().Product.Inventory).Eager
                        .Fetch(x => x.Returns).Eager
                        .Fetch(x => x.Returns.First().Product).Eager
                        .Fetch(x => x.Returns.First().Product.Inventory).Eager
                        .FutureValue();

                    var entity = query.Value;
                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
