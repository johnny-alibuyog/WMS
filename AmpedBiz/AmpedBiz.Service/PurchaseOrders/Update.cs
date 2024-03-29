﻿using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class Update
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

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
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    //entity.CompletedBy = session.Load<Employee>(message.CompletedByEmployeeId);
                    //entity.CreatedBy = session.Load<Employee>(message.CreatedByEmployeeId);
                    //entity.Payment = new Money(message.PaymentAmount, currency);
                    //entity.PaymentType = session.Load<PaymentType>(message.PaymentTypeId);
                    //entity.SubmittedBy = session.Load<Employee>(message.SubmittedByEmployeeId);
                    //entity.SubTotal = new Money(message.SubTotalAmount, currency);
                    //entity.Supplier = session.Load<Supplier>(message.SupplierId);
                    //entity.Tax = new Money(message.TaxAmount, currency);
                    //entity.Total = new Money(message.TotalAmount, currency);

                    message.MapTo(entity);

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}