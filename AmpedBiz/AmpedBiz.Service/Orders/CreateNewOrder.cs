using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class CreateNewOrder
    {
        public class Request : Dto.OrderNewlyCreatedEvent, IRequest<Response>
        {
            public virtual Guid UserId { get; set; }
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
                    var exists = session.Query<Order>().Any(x => x.Id == message.OrderId);
                    if (exists)
                        throw new BusinessException($"Order with id {message.OrderId} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var entity = message.MapTo(new Order(message.Id));

                    var newlyCreatedEvent = new OrderNewlyCreatedEvent(
                        createdBy: (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        createdOn: message?.CreatedOn ?? DateTime.Now,
                        branch: (!message?.Branch?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Branch>(message.Branch.Id) : null,
                        customer: (!message?.Customer?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Customer>(message.Customer.Id) : null,
                        shipper: (!message?.Shipper?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Shipper>(message.Shipper.Id) : null,
                        paymentType: (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        taxRate: message.TaxRate,
                        tax: new Money(message.TaxAmount, currency),
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        discount: new Money(message.DiscountAmount, currency),
                        items: message.Items
                            .Select(x => new OrderItem(
                                product: session.Get<Product>(x.Product.Id),
                                discount: new Money(x.DiscountAmount, currency),
                                unitPrice: new Money(x.UnitPriceAmount, currency),
                                quantity: new Measure(x.QuantityValue, 
                                    session.Get<Product>(x.Product.Id).Inventory.UnitOfMeasure)
                            ))
                    );

                    entity.State.Process(newlyCreatedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}