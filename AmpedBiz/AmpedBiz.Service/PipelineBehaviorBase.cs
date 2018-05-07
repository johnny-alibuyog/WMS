using AmpedBiz.Data.Context;
using MediatR;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace AmpedBiz.Service
{
    public abstract class PipelineBehaviorBase<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public IContext context { get; set; }

        public ISessionFactory sessionFactory { get; set; }

        public abstract Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next);
    }
}
