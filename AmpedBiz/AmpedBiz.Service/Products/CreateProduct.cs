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
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<Product>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Product with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.Product, Product>(message);

                    entity.BasePrice = new Money()
                    {
                        Amount = message.BasePriceAmount,
                        Currency = session.Load<Currency>(message.BasePriceCurrencyId)
                    };
                    entity.RetailPrice = new Money()
                    {
                        Amount = message.RetailPriceAmount,
                        Currency = session.Load<Currency>(message.RetailPriceCurrencyId)
                    };
                    entity.WholeSalePrice = new Money()
                    {
                        Amount = message.WholeSalePriceAmount,
                        Currency = session.Load<Currency>(message.WholeSalePriceCurrencyId)
                    };

                    entity.Supplier = new Supplier()
                    {
                        Id = message.SupplierId
                    };

                    entity.Category = new ProductCategory()
                    {
                        Id = message.CategoryId
                    };

                    session.Save(entity);

                    Mapper.Map<Product, Dto.Product>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
