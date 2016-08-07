﻿using AmpedBiz.Common.Exceptions;
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

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory) : base(sessionFactory) { }

            public override Response Handle(Request message)
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
                    entity.WholeSalePrice = new Money(message.RetailPriceAmount, currency);
                    entity.Supplier = session.Load<Supplier>(message.Supplier.Id);
                    entity.Category = session.Load<ProductCategory>(message.Category.Id);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}