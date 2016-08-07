using AmpedBiz.Common.Exceptions;
using AmpedBiz.Common.Extentions;
using AmpedBiz.Core.Entities;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using System.Linq;

namespace AmpedBiz.Service.Products
{
    public class GetProduct
    {
        public class Request : IRequest<Response>
        {
            public string Id { get; set; }
        }

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
                    var entity = session.Query<Product>()
                        .Where(x => x.Id == message.Id)
                        .Fetch(x => x.Supplier)
                        .Fetch(x => x.Category)
                        .FirstOrDefault();

                    if (entity == null)
                        throw new BusinessException($"Product with id {message.Id} does not exists.");

                    entity.MapTo(response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}