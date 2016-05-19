using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class UpdatePurchaseOrder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = session.Get<PurchaseOrder>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} does not exists.");

                    Mapper.Map<Dto.PurchaseOrder, PurchaseOrder>(message, entity);

                    entity.CompletedBy = session.Load<Employee>(message.CompletedByEmployeeId);
                    entity.CreatedBy = session.Load<Employee>(message.CreatedByEmployeeId);
                    entity.Payment = new Money(message.PaymentAmount, currency);
                    entity.PaymentType = session.Load<PaymentType>(message.PaymentTypeId);
                    entity.SubmittedBy = session.Load<Employee>(message.SubmittedByEmployeeId);
                    entity.SubTotal = new Money(message.SubTotalAmount, currency);
                    entity.Supplier = session.Load<Supplier>(message.SupplierId);
                    entity.Tax = new Money(message.TaxAmount, currency);
                    entity.Total = new Money(message.TotalAmount, currency);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}