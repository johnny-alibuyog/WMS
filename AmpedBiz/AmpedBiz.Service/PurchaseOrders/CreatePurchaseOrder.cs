using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CreatePurchaseOrder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response>
        {
            public virtual string EmployeeId { get; set; }
        }

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
                    var exists = session.Query<PurchaseOrder>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = message.MapTo(new PurchaseOrder(message.Id));

                    entity.CurrentState.New(
                        createdBy: session.Load<Employee>(message.EmployeeId), 
                        createdOn: DateTime.Now, 
                        paymentType: session.Load<PaymentType>(message.PaymentTypeId), 
                        shipper: null, 
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        tax: new Money(message.TaxAmount, currency), 
                        supplier: session.Load<Supplier>(message.SupplierId)
                    );
                    
                    foreach(var item in message.PurchaseOrderDetails)
                    {
                        var detail = new PurchaseOrderDetail(item.Id);
                        detail.CurrentState.New(
                            product: session.Load<Product>(item.ProductId),
                            unitPrice: new Money(item.UnitPriceAmount, currency),
                            quantity: item.QuantityValue
                        );

                        item.MapTo(detail);

                        entity.AddPurchaseOrderDetail(detail);
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