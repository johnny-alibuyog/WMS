﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Arguments.Orders;
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

                    var newlyCreatedArguments = new OrderNewlyCreatedArguments()
                    {
                        CreatedBy = (!message?.CreatedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.CreatedBy.Id) : null,
                        CreatedOn = message?.CreatedOn ?? DateTime.Now,
                        OrderedBy = (!message?.OrderedBy?.Id.IsNullOrDefault() ?? false)
                            ? session.Load<User>(message.OrderedBy.Id) : null,
                        OrderedOn = message?.OrderedOn ?? DateTime.Now,
                        Branch = (!message?.Branch?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Branch>(message.Branch.Id) : null,
                        Customer = (!message?.Customer?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Customer>(message.Customer.Id) : null,
                        PricingScheme = (!message?.PricingScheme?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PricingScheme>(message.PricingScheme.Id) : null,
                        Shipper = (!message?.Shipper?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<Shipper>(message.Shipper.Id) : null,
                        ShippingAddress = (message.ShippingAddress != null)
                            ? message.ShippingAddress.MapTo<Dto.Address, Address>() : null,
                        PaymentType = (!message?.PaymentType?.Id.IsNullOrEmpty() ?? false)
                            ? session.Load<PaymentType>(message.PaymentType.Id) : null,
                        TaxRate = message.TaxRate,
                        Tax = new Money(message.TaxAmount, currency),
                        ShippingFee = new Money(message.ShippingFeeAmount, currency),
                        Items = message.Items
                            .Select(x => new OrderItem(
                                id: x.Id,
                                product: session.Get<Product>(x.Product.Id),
                                discount: new Money(x.DiscountAmount, currency),
                                unitPrice: new Money(x.UnitPriceAmount, currency),
                                quantity: new Measure(x.QuantityValue,
                                    session.Get<Product>(x.Product.Id).Inventory.UnitOfMeasure)
                            ))
                    };

                    entity.State.Process(newlyCreatedArguments);

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