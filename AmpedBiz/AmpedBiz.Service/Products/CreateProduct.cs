using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class CreateProduct
    {
        public class Request : Dto.Product, IRequest<Response> { }

        public class Response : Dto.Product { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Product>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Product with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = message.MapTo(new Product(message.Id));
                    entity.BasePrice = new Money(message.BasePriceAmount, currency);
                    entity.RetailPrice = new Money(message.RetailPriceAmount, currency);
                    entity.WholeSalePrice = new Money(message.WholesalePriceAmount, currency);
                    entity.Supplier = session.Load<Supplier>(message.Supplier.Id);
                    entity.Category = session.Load<ProductCategory>(message.Category.Id);

                    session.Save(entity);
                    transaction.Commit();

                    response = entity.MapTo(response);;
                }

                return response;
            }
        }
    }
}
