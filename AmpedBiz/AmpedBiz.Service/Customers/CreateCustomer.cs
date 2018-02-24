using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using MediatR;
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
            public override Response Execute(Request message)
            {
                var response = new Response();

                using (var session = SessionFactory.RetrieveSharedSession(Context))
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Customer>().Any(x => x.Id == message.Id);
                    exists.Assert($"Customer with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id);
                    var entity = message.MapTo(new Customer(message.Id));
                    entity.CreditLimit = new Money(message.CreditLimitAmount, currency);
                    entity.Pricing = session.Load<Pricing>(
                        string.IsNullOrEmpty(message.PricingId)
                            ? Pricing.RetailPrice.Id 
                            : message.PricingId
                    );
                    entity.EnsureValidity();

                    session.Save(entity);
                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}