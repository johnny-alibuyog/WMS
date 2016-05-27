using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

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
                    entity.Tax = new Money(message.TaxAmount, currency);
                    entity.ShippingFee = new Money(message.ShippingFeeAmount, currency);
                    entity.Payment = new Money(message.PaymentAmount, currency);
                    entity.SubTotal = new Money(message.SubTotalAmount, currency);
                    entity.Total = new Money(message.TotalAmount, currency);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}