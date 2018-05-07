using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Middlewares
{
    public class LogginPipeline<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse>
    {
        public override Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var result = next();
            stopwatch.Stop();

            Console.WriteLine($"Logging: {stopwatch.Elapsed}");

            return result;
        }
    }
}
