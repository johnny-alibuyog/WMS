﻿using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Services.Orders;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
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
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = (Order)null;

                    if (message.Id != Guid.Empty)
                    {
                        entity = session.QueryOver<Order>()
                            .Where(x => x.Id == message.Id)
                            .Fetch(x => x.Branch).Eager
                            .Fetch(x => x.Customer).Eager
                            .Fetch(x => x.Pricing).Eager
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
                            .Fetch(x => x.Payments.First().PaidTo).Eager
                            .Fetch(x => x.Payments.First().PaymentType).Eager
                            .Fetch(x => x.Returns).Eager
                            .Fetch(x => x.Returns.First().Reason).Eager
                            .Fetch(x => x.Returns.First().ReturnedBy).Eager
                            .Fetch(x => x.Returns.First().Product).Eager
                            .Fetch(x => x.Returns.First().Product.Inventory).Eager
                            .SingleOrDefault();

                        entity.EnsureExistence($"Order with id {message.Id} does not exists.");
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
                        .FetchMany(x => x.UnitOfMeasures)
                        .ThenFetchMany(x => x.Prices)
                        .ToList();

                    Func<Guid, Product> GetProduct = (id) => products.First(x => x.Id == id);

                    entity.Accept(new OrderUpdateVisitor()
                    {
                        OrderNumber = message.OrderNumber,
                        CreatedBy = (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        CreatedOn = message?.CreatedOn ?? DateTime.Now,
                        OrderedBy = (!message?.OrderedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        OrderedOn = message?.OrderedOn ?? DateTime.Now,
                        Branch = (!message?.Branch?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Branch>(message.Branch.Id) : null,
                        Customer = (!message?.Customer?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<Customer>(message.Customer.Id) : null,
                        Shipper = (!message?.Shipper?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Shipper>(message.Shipper.Id) : null,
                        ShippingAddress = (message.ShippingAddress != null)
                            ? message.ShippingAddress.MapTo<Dto.Address, Address>() : null,
                        Pricing = (!message?.Pricing?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Pricing>(message.Pricing.Id) : null,
                        PaymentType = (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        TaxRate = message.TaxRate,
                        Tax = new Money(message.TaxAmount, currency),
                        ShippingFee = new Money(message.ShippingFeeAmount, currency),
                        Items = message.Items
                            .Select(x => new OrderItem(
                                id: x.Id,
                                discountRate: x.DiscountRate,
                                product: GetProduct(x.Product.Id),
                                unitPrice: new Money(x.UnitPriceAmount, currency),
                                quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
                                standard: new Measure(x.Standard.Value, session.Load<UnitOfMeasure>(x.Standard.Unit.Id))
                            ))
                            .ToList(),
                        Payments = message.Payments
                            .Select(x => new OrderPayment(
                                id: x.Id,
                                paidOn: x.PaidOn ?? DateTime.Now,
                                paidTo: session.Load<User>(x.PaidTo.Id),
                                paymentType: session.Load<PaymentType>(x.PaymentType.Id),
                                payment: new Money(x.PaymentAmount, currency)
                            ))
                            .ToList(),
                        Returns = message.Returns
                            .Select(x => new OrderReturn(
                                id: x.Id,
                                product: GetProduct(x.Product.Id),
                                reason: session.Load<ReturnReason>(x.Reason.Id),
                                returnedOn: message.ReturnedOn ?? DateTime.Now,
                                returnedBy: session.Load<User>(x.ReturnedBy.Id),
                                quantity: new Measure(x.Quantity.Value, session.Load<UnitOfMeasure>(x.Quantity.Unit.Id)),
                                returned: new Money(x.ReturnedAmount, currency)
                            ))
                            .ToList()
                    });
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}