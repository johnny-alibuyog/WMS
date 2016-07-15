using System;
using System.Collections.Generic;
using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Orders
{
    public class InvoiceOrder
    {
        public class Request : Dto.Order, IRequest<Response>
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
                    var entity = session.Get<Order>(message.Id);
                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    if (entity == null)
                        throw new BusinessException($"Order with id {message.Id} does not exists.");

                    var user = session.Load<User>(message.UserId);

                    var invoices = new List<Invoice>();

                    //not working
                    //message.Invoices.MapTo(invoices);
                    foreach(var i in message.Invoices)
                    {
                        invoices.Add(new Invoice()
                        {
                            DueDate = i.DueDate,
                            InvoiceDate = i.InvoiceDate,
                            Shipping = new Money(i.ShippingAmount.Value),
                            SubTotal = new Money(i.SubTotalAmount.Value),
                            Tax = new Money(i.TaxAmount.Value),
                            Total = new Money(i.TotalAmount.Value)
                        });
                    }

                    entity.State.Invoice(user, invoices);

                    session.Save(entity);
                    transaction.Commit();

                    //todo: not working when mapped to invoice
                    //entity.MapTo(response);
                    response.Id = entity.Id;
                    response.Status = entity.Status == OrderStatus.Invoiced ? Dto.OrderStatus.Invoiced : Dto.OrderStatus.New;
                }

                return response;
            }
        }
    }
}
