using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Orders
{
    public class UpdateNewOrder
    {
        public class Request : Dto.Order, IRequest<Response> { }

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
                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var entity = message.MapTo(new Order(message.Id));

                    entity.State.New(
                        createdBy: session.Load<User>(message.CreatedById),
                        paymentType: session.Load<PaymentType>(message.PaymentTypeId),
                        shipper: null,
                        shippingFee: new Money(message.ShippingFeeAmount, currency),
                        taxRate: message.TaxRate,
                        customer: session.Load<Customer>(message.CustomerId),
                        branch: session.Load<Branch>(message.BranchId)
                    );

                    session.Save(entity);

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}