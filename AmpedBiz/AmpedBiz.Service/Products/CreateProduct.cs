using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Products
{
    public class CreateProduct
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
                if (string.IsNullOrEmpty(message.Image))
                    message.Image = string.Empty;

                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Product>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Product with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = Mapper.Map<Dto.Product, Product>(message, new Product(message.Id));
                    entity.BasePrice = new Money(message.BasePriceAmount, currency);
                    entity.RetailPrice = new Money(message.RetailPriceAmount, currency);
                    entity.WholesalePrice = new Money(message.RetailPriceAmount, currency);
                    entity.Supplier = session.Load<Supplier>(message.SupplierId);
                    entity.Category = session.Load<ProductCategory>(message.CategoryId);

                    session.Save(entity);

                    Mapper.Map<Product, Dto.Product>(entity, response);
                    //todo: cannot map amounts and id
                    response.BasePriceAmount = entity.BasePrice.Amount;
                    response.WholesalePriceAmount = entity.WholesalePrice.Amount;
                    response.RetailPriceAmount = entity.RetailPrice.Amount;
                    response.CategoryId = entity.Category.Id;
                    response.SupplierId = entity.Supplier.Id;

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
