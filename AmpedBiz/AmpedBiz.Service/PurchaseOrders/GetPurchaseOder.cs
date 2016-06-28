using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;


namespace AmpedBiz.Service.PurchaseOrders
{
    public class GetPurchaseOder
    {
        public class Request : IRequest<Response>
        {
            public Guid Id { get; set; }
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
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    response = new Response()
                    {
                        Id = entity.Id,
                        SupplierId = entity.Supplier.Id,
                        PaymentTypeId = entity.PaymentType.Id,
                        TaxAmount = entity.Tax.Amount,
                        ShippingFeeAmount = entity.ShippingFee.Amount,
                        PaymentAmount = entity.Payment.Amount,
                        SubTotalAmount = entity.SubTotal.Amount,
                        TotalAmount = entity.Total.Amount,
                        Status = entity.Status.Parse<Dto.PurchaseOrderStatus>(),
                        ExpectedOn = entity.ExpectedOn,
                    };

                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}