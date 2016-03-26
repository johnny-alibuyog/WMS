using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service.ProductCategories
{
    public class UpdateProductCategory
    {
        public class Request : Dto.ProductCategory, IRequest<Response> { }

        public class Response : Dto.ProductCategory { }

        public class Handler : IRequestHandler<Request, Response>
        {
            private readonly ISessionFactory _sessionFactory;

            public Handler(ISessionFactory sessionFactory)
            {
                _sessionFactory = sessionFactory;
            }

            public Response Handle(Request message)
            {
                var response = default(Response);

                using (var session = _sessionFactory.OpenSession())
                using (var transaction = session.BeginTransaction())
                {
                    var entity = session.Get<ProductCategory>(message.Id);
                    if (entity == null)
                        throw new BusinessException($"Product Category with id {message.Id} does not exists.");

                    entity.Name = message.Name;

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
