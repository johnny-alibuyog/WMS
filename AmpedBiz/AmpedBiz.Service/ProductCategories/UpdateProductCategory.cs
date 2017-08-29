using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using AmpedBiz.Data;
using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.ProductCategories
{
    public class UpdateProductCategory
    {
        public class Request : Dto.ProductCategory, IRequest<Response> { }

        public class Response : Dto.ProductCategory { }

        public class Handler : RequestHandlerBase<Request, Response>
        {
            public Handler(ISessionFactory sessionFactory, IContext context) : base(sessionFactory, context) { }

            public override Response Handle(Request message)
            {
                var response = new Response();

                using (var session = _sessionFactory.RetrieveSharedSession(_context))
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<ProductCategory>(message.Id);
                    entity.EnsureExistence($"Product Category with id {message.Id} does not exists.");
                    entity.MapFrom(message);

                    transaction.Commit();

                    entity.MapTo(response);
                }

                return response;
            }
        }
    }
}
