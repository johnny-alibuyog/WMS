using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOder
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

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
                    var query = session.Query<PurchaseOrder>()
                        .Where(x => x.Id == message.Id)
                        .FetchMany(x => x.Items)
                        .Fetch(x => x.Tax)
                        .Fetch(x => x.ShippingFee)
                        .Fetch(x => x.Payment)
                        .Fetch(x => x.SubTotal)
                        .Fetch(x => x.Total)
                        .Fetch(x => x.CreatedBy)
                        .Fetch(x => x.SubmittedBy)
                        .Fetch(x => x.ApprovedBy)
                        .Fetch(x => x.PaidBy)
                        .Fetch(x => x.CompletedBy)
                        .Fetch(x => x.CancelledBy)
                        .ToFutureValue();

                    var entity = query.Value;
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}