﻿using System;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Orders
{
    public class UpdateNewOrder
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
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = session.Get<Order>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    entity.State.New(
                        createdBy: !message.CreatedById.IsNullOrDefault()
                            ? session.Load<User>(message.UserId) : null,
                        paymentType: session.Load<PaymentType>(message.PaymentTypeId),
                        shipper: null,
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        taxRate: message.TaxRate,
                        customer: session.Load<Customer>(message.CustomerId),
                        branch: session.Load<Branch>(message.BranchId)
                    );

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}