using System;
using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;

namespace AmpedBiz.Service.Orders
{
    public class GetOrder
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }

            public Request() { }

            public Request(Guid id)
            {
                this.Id = id;
            }
        }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                if (message.Id == Guid.Empty)
                {
                    var entity = new Order();
                    entity.MapTo(response);

                    return response;
                }

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var query = session.QueryOver<Order>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Tax).Eager
                        .Fetch(x => x.ShippingFee).Eager
                        .Fetch(x => x.Discount).Eager
                        .Fetch(x => x.SubTotal).Eager
                        .Fetch(x => x.Total).Eager
                        .Fetch(x => x.CreatedBy).Eager
                        .Fetch(x => x.RoutedBy).Eager
                        .Fetch(x => x.StagedBy).Eager
                        .Fetch(x => x.InvoicedBy).Eager
                        .Fetch(x => x.PaidTo).Eager
                        .Fetch(x => x.CompletedBy).Eager
                        .Fetch(x => x.CancelledBy).Eager
                        .Fetch(x => x.Items).Eager
                        .Fetch(x => x.Items.First().Product).Eager
                        .Fetch(x => x.Payments).Eager
                        .Fetch(x => x.Payments.First().PaidBy).Eager
                        .Fetch(x => x.Payments.First().PaymentType).Eager
                        .TransformUsing(Transformers.DistinctRootEntity)
                        .FutureValue();

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