using System.Linq;
using AmpedBiz.Common.Exceptions;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;
using Dto = AmpedBiz.Service.Dto;
using Entity = AmpedBiz.Core.Entities;

namespace AmpedBiz.Service.Branches
{
    public class CreateBranch
    {
        public class Request : Dto.Branch, IRequest<Response> { }

        public class Response : Dto.Branch { }

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
                    var exists = session.Query<Entity.Branch>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"Branch with id {message.Id} already exists.");

                    var entity = Mapper.Map<Dto.Branch, Entity.Branch>(message);

                    session.Save(entity);

                    Mapper.Map<Entity.Branch, Dto.Branch>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}
