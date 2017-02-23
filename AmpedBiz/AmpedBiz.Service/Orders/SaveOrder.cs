using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Linq;

namespace AmpedBiz.Service.Orders
{
    public class SaveOrder
    {
        public class Request : Dto.Order, IRequest<Response>
        {
            public virtual Guid UserId { get; set; }
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
                    var entity = (Order)null;

                    if (message.Id != Guid.Empty)
                    {
                        entity = session.QueryOver<Order>()
                            .Where(x => x.Id == message.Id)
                            .Fetch(x => x.Branch).Eager
                            .Fetch(x => x.Customer).Eager
                            .Fetch(x => x.PricingScheme).Eager
                            .Fetch(x => x.PaymentType).Eager
                            .Fetch(x => x.Shipper).Eager
                            .Fetch(x => x.Tax).Eager
                            .Fetch(x => x.ShippingFee).Eager
                            .Fetch(x => x.Discount).Eager
                            .Fetch(x => x.SubTotal).Eager
                            .Fetch(x => x.Total).Eager
                            .Fetch(x => x.CreatedBy).Eager
                            .Fetch(x => x.OrderedBy).Eager
                            .Fetch(x => x.RoutedBy).Eager
                            .Fetch(x => x.StagedBy).Eager
                            .Fetch(x => x.InvoicedBy).Eager
                            .Fetch(x => x.PaidTo).Eager
                            .Fetch(x => x.RoutedBy).Eager
                            .Fetch(x => x.CompletedBy).Eager
                            .Fetch(x => x.CancelledBy).Eager
                            .Fetch(x => x.Items).Eager
                            .Fetch(x => x.Items.First().Product).Eager
                            .Fetch(x => x.Items.First().Product.Inventory).Eager
                            .Fetch(x => x.Payments).Eager
                            .Fetch(x => x.Payments.First().PaidBy).Eager
                            .Fetch(x => x.Payments.First().PaymentType).Eager
                            .Fetch(x => x.Returns).Eager
                            .Fetch(x => x.Returns.First().ReturnedBy).Eager
                            .Fetch(x => x.Returns.First().Product).Eager
                            .Fetch(x => x.Returns.First().Product.Inventory).Eager
                            .SingleOrDefault();

                        if (entity == null)
                            throw new BusinessException($"Order with id {message.Id} does not exists.");
                    }
                    else
                    {
                        entity = new Order();
                    }

                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    var productIds = message.Items.Select(x => x.Product.Id);

                    var products = session.Query<Product>()
                        .Where(x => productIds.Contains(x.Id))
                        .Fetch(x => x.Inventory)
                        .ToList();

                    Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    Func<Guid, UnitOfMeasure> GetUnitOfMeasure = (id) => products.First(x => x.Id == id).Inventory.UnitOfMeasure;

                    entity.Accept(new OrderSaveVisitor()
                    {
                        OrderNumber = message.OrderNumber,
                        CreatedBy = (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        CreatedOn = message?.CreatedOn ?? DateTime.Now,
                        OrderedBy = (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        OrderedOn = message?.CreatedOn ?? DateTime.Now,
                        Branch = (!message?.Branch?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Branch>(message.Branch.Id) : null,
                        Customer = (!message?.Customer?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Customer>(message.Customer.Id) : null,
                        Shipper = (!message?.Shipper?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Shipper>(message.Shipper.Id) : null,
                        ShippingAddress = (message.ShippingAddress != null)
                            ? message.ShippingAddress.MapTo<Dto.Address, Address>() : null,
                        PricingScheme = (!message?.PricingScheme?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PricingScheme>(message.PricingScheme.Id) : null,
                        PaymentType = (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        TaxRate = message.TaxRate,
                        Tax = new Money(message.TaxAmount, currency),
                        ShippingFee = new Money(message.ShippingFeeAmount, currency),
                        Items = message.Items.Select(x => new OrderItem(
                            id: x.Id,
                            discountRate: x.DiscountRate,
                            product: GetProduct(x.Product.Id),
                            discount: new Money(x.DiscountAmount, currency),
                            unitPrice: new Money(x.UnitPriceAmount, currency),
                            quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id))
                        )),
                        Payments = message.Payments.Select(x => new OrderPayment(
                            paidOn: x.PaidOn ?? DateTime.Now,
                            paidBy: session.Load<User>(x.PaidBy.Id),
                            paymentType: session.Load<PaymentType>(x.PaymentType.Id),
                            payment: new Money(x.PaymentAmount, currency)
                        )),
                        Returns = message.Returns.Select(x => new OrderReturn(
                            product: GetProduct(x.Product.Id),
                            returnedOn: message.ReturnedOn ?? DateTime.Now,
                            returnedBy: session.Load<User>(x.ReturnedBy.Id),
                            quantity: new Measure(x.QuantityValue, GetUnitOfMeasure(x.Product.Id)),
                            discountRate: x.DiscountRate,
                            discount: new Money(x.DiscountAmount, currency),
                            unitPrice: new Money(x.UnitPriceAmount, currency)
                        ))
                    });

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}