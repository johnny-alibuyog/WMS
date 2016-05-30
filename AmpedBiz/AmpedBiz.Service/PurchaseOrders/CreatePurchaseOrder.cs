using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;
using System;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CreatePurchaseOrder
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
                    var exists = session.Query<PurchaseOrder>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = message.MapTo(new PurchaseOrder(message.Id));

                    var tax = new Money(message.TaxAmount, currency);
                    var shippingFee = new Money(message.ShippingFeeAmount, currency);
                    var payment = new Money(message.PaymentAmount, currency);
                    var supTotal = new Money(message.SubTotalAmount, currency);
                    var total = new Money(message.TotalAmount, currency);
                    var createdBy = session.Load<Employee>(message.CreatedByEmployeeId);
                    var supplier = session.Load<Supplier>(message.SupplierId); ;
                    var paymentType = session.Load<PaymentType>(message.PaymentTypeId);

                    entity.New(payment, paymentType, null, tax, shippingFee, createdBy, supplier);
                    
                    foreach(var poDetail in message.PurchaseOrderDetails)
                    {
                        var detail = new PurchaseOrderDetail(poDetail.Id);
                        var product = session.Load<Product>(poDetail.ProductId);

                        detail.Product = product;
                        detail.UnitCost = new Money(poDetail.UnitCostAmount, currency);
                        detail.ExtendedPrice = new Money(poDetail.ExtendedPriceAmount, currency);

                        entity.AddPurchaseOrderDetail(poDetail.MapTo(detail));
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