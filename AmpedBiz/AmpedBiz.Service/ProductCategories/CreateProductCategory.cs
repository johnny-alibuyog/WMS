using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.ProductCategories
{
    public class CreateProductCategory
    {
        public class Request : Dto.ProductCategory, IRequest<Response> { }

        public class Response : Dto.ProductCategory { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var exists = session.Query<ProductCategory>().Any(x => x.Id == message.Id);
                    exists.Assert($"Product Category with id {message.Id} already exists.");

                    var entity = message.MapTo(new ProductCategory(message.Id));
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
