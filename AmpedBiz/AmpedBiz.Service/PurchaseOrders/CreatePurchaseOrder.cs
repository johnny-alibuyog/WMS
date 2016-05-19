using System.Linq;
using AmpedBiz.Common.Exceptions;
using AmpedBiz.Core.Entities;
using ExpressMapper;
using MediatR;
using NHibernate;
using NHibernate.Linq;

namespace AmpedBiz.Service.PurchaseOrders
{
    public class CreatePurchaseOrder
    {
        public class Request : Dto.PurchaseOrder, IRequest<Response> { }

        public class Response : Dto.PurchaseOrder { }

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
                    var exists = session.Query<PurchaseOrder>().Any(x => x.Id == message.Id);
                    if (exists)
                        throw new BusinessException($"PurchaseOrder with id {message.Id} already exists.");

                    var currency = session.Load<Currency>(Currency.PHP.Id); // this should be taken from the tenant
                    var entity = Mapper.Map<Dto.PurchaseOrder, PurchaseOrder>(message, new PurchaseOrder(message.Id));
                    session.Save(entity);

                    Mapper.Map<PurchaseOrder, Dto.PurchaseOrder>(entity, response);

                    transaction.Commit();
                }

                return response;
            }
        }
    }
}