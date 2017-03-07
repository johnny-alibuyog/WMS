using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Customers
{
    public class CreateCustomer
    {
        public class Request : Dto.Customer, IRequest<Response> { }

        public class Response : Dto.Customer { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Customer>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Customer with id {message.Id} already exists.");

                    if (string.IsNullOrEmpty(message.PricingId))
                        message.PricingId = "RP";

                    var entity = message.MapTo(new Customer(message.Id));
                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    entity.CreditLimit = new Money(message.CreditLimitAmount, currency);
                    entity.Pricing = session.Load<Pricing>(message.PricingId);

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}