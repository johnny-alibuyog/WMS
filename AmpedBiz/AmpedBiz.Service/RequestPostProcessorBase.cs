using AmpedBiz.Data.Context;
using MediatR;
using MediatR.Pipeline;
using NHibernate;
using System.Threading;
using System.Threading.Tasks;

namespace AmpedBiz.Service
{
    public abstract class RequestPostProcessorBase<TRequest, TResponse> : IRequestPostProcessor<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        public ISessionFactory sessionFactory { get; set; }

        public IContext context { get; set; }

        public abstract Task Execute(TRequest request, TResponse response);

        public Task Process(TRequest request, TResponse response, CancellationToken cancellationToken)
        {
            return Execute(request, response);
        }
    }

    public static class RequestPostProcessorExtention
    {
        public static RequestPostProcessorBase<TRequest, TResponse> With<TRequest, TResponse>(this RequestPostProcessorBase<TRequest, TResponse> handler, ISessionFactory sessionFactory, IContext context)
            where TRequest : IRequest<TResponse>
        {
            handler.sessionFactory = sessionFactory;
            handler.context = context;
            return handler;
        }
    }
}
