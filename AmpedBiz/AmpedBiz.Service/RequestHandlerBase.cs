using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;

namespace AmpedBiz.Service
{
    public abstract class RequestHandlerBase<TRequest, TResponse> : IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly IContext _context;
        protected readonly ISessionFactory _sessionFactory;

        public abstract TResponse Handle(TRequest message);

        public RequestHandlerBase(ISessionFactory sessionFactory, IContext context)
        {
            this._context = context;
            this._sessionFactory = sessionFactory;
        }
    }
}
