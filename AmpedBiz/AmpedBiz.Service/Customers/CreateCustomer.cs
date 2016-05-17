using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
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
                    var exists = session.Query<Customer>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Customer with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.Customer, Customer>(message, new Customer(message.Id));
                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    entity.CreditLimit = new Money(message.CreditLimitAmount, currency);
                    entity.PricingScheme = session.Load<PricingScheme>(message.PricingSchemeId);

                    session.Save(entity);

                    Mapper.Map<Customer, Dto.Customer>(entity, response);

                    //todo: cannot map
                    response.PricingSchemeId = entity.PricingScheme.Id;
                    response.CreditLimitAmount = entity.CreditLimit.Amount;

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}