using System.Linq;
using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Suppliers
{
    public class CreateSupplier
    {
        public class Request : Dto.Supplier, IRequest<Response> { }

        public class Response : Dto.Supplier { }

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
                    var exists = session.Query<Entity.Supplier>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Supplier with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.Supplier, Entity.Supplier>(message, new Entity.Supplier(message.Id));

                    session.Save(entity);

                    Mapper.Map<Entity.Supplier, Dto.Supplier>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
