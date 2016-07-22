using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Core.Events.Orders;
using MediatR;
using NHibernate;
using System;
using System.Linq;
using System.Collections.Generic;

namespace AmpedBiz.Service.Orders
{
    public class InvoiceOrder
    {
        public class Request : Dto.OrderInvoicedEvent, IRequest<Response> { }

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
                    var entity = session.Get<Order>(message.OrderId);
                    var currency = session.Load<Currency>(Currency.PHP.Id);

                    if (entity == null)
                        throw new BusinessException($"Order with id {message.OrderId} does not exists.");

                    var invoicedEvent = new OrderInvoicedEvent(
                        invoices: message.Invoices.Select(x => new OrderInvoice(
                            invoicedOn: x.InvoicedOn ?? DateTime.Now,
                            invoicedBy: session.Load<User>(x.InvoicedBy.Id),
                            dueOn: x.DueOn,
                            tax: new Money(x.TaxAmount, currency),
                            shipping: new Money(x.ShippingAmount, currency),
                            dicount: new Money(x.DiscountAmount, currency),
                            subTotal: new Money(x.SubTotalAmount, currency)
                        )
                    ));

                    entity.State.Process(invoicedEvent);

                    session.Save(entity);
                    transaction.Commit();

                    //todo: not working when mapped to invoice
                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
