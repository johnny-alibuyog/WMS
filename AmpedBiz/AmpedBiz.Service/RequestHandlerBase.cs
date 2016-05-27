using MediatR;
using NHibernate;

namespace AmpedBiz.Service
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly ISessionFactory _sessionFactory;

        public abstract TResponse Handle(TRequest message);

        public RequestHandlerBase(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
    }
}
