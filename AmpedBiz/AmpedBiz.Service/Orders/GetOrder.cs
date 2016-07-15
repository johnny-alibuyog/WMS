using System;
using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.Orders
{
    public class GetOrder
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
        }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.Query<Order>()
                        .Where(x => x.Id == message.Id)
                        .FetchMany(x => x.Items)
                        .Fetch(x => x.Tax)
                        .Fetch(x => x.ShippingFee)
                        .Fetch(x => x.SubTotal)
                        .Fetch(x => x.Total)
                        .Fetch(x => x.CreatedBy)
                        .Fetch(x => x.RoutedBy)
                        .Fetch(x => x.StagedBy)
                        .Fetch(x => x.InvoicedBy)
                        .Fetch(x => x.PartiallyPaidBy)
                        .Fetch(x => x.CompletedBy)
                        .Fetch(x => x.CancelledBy)
                        .ToFutureValue();

                    var entity = query.Value;
                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}