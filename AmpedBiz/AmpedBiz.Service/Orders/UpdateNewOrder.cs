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
    public class UpdateNewOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

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
                    var entity = session.Get<Order>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant

                    var productIds = message.Items.Select(x => x.Product.Id);

                    var products = session.Query<Product>()
                        .Where(x => productIds.Contains(x.Id))
                        .Fetch(x => x.Inventory)
                        .ToList();

                    Func<string, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<string, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    var newlyCreatedEvent = new OrderNewlyCreatedEvent(
                        createdBy: (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        createdOn: message?.CreatedOn ?? DateTime.Now,
                        branch: (!message?.Branch?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Branch>(message.Branch.Id) : null,
                        customer: (!message?.Customer?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Customer>(message.Customer.Id) : null,
                        pricingScheme: (!message?.PricingScheme?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PricingScheme>(message.PricingScheme.Id) : null,
                        shipper: (!message?.Shipper?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Shipper>(message.Shipper.Id) : null,
                        shippingAddress: (message.ShippingAddress != null)
                            ? message.ShippingAddress.MapTo<Dto.Address, Address>() : null,
                        paymentType: (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        taxRate: message.TaxRate,
                        tax: new Money(message.TaxAmount, currency),
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        items: message.Items
                            .Select(x => new OrderItem(
                                product: GetProduct(x.Product.Id),
                                discount: new Money(x.DiscountAmount, currency),
                                unitPrice: new Money(x.UnitPriceAmount, currency),
                                quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id))
                            ))
                    );

                    entity.State.Process(newlyCreatedEvent);

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}