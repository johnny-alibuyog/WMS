using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.Products
{
    public class UpdateProduct
    {
        public class Request : Dto.Product, IRequest<Response> { }

        public class Response : Dto.Product { }

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
                    var entity = session.Get<Product>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Product with id {message.Id} does not exists.");

                    message.MapTo(entity);
                    entity.BasePrice = new Money(message.BasePriceAmount, currency);
                    entity.RetailPrice = new Money(message.RetailPriceAmount, currency);
                    entity.WholesalePrice = new Money(message.RetailPriceAmount, currency);
                    entity.Supplier = session.Load<Supplier>(message.SupplierId);
                    entity.Category = session.Load<ProductCategory>(message.CategoryId);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}