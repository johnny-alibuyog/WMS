using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.Orders
{
    public class CreateNewOrder
    {
        public class Request : Dto.Order, IRequest<Response>
        {
            public virtual string UserId { get; set; }
        }

        public class Response : Dto.Order { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory)
            {
            }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Order>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Order with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var entity = message.MapTo(new Order(message.Id));

                    entity.State.New(
                        createdBy: session.Load<User>(message.UserId),
                        paymentType: session.Load<PaymentType>(message.PaymentTypeId),
                        shipper: null,
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        taxRate: message.TaxRate,
                        customer: session.Load<Customer>(message.CustomerId),
                        branch: session.Load<Branch>(message.BranchId)
                    );

                    foreach (var item in message.OrderItems)
                    {
                        var orderItem = new OrderItem(item.Id);
                        orderItem.CurrentState.Allocate();

                        item.MapTo(orderItem);

                        entity.AddOrderItem(orderItem);
                    }

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}