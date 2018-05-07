using MediatR;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace AmpedBiz.Service.Middlewares
{
    public class TransactionPipeline<TRequest, TResponse> : PipelineBehaviorBase<TRequest, TResponse>
    {
        public override Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //http://www.hurryupandwait.io/blog/getting-transactionscope-to-play-nice-with-nhibernate

            var stopwatch = new Stopwatch();

            stopwatch.Start();

            //using (var transaction = new TransactionScope())
            //{
            var result = next();
            //}

            stopwatch.Stop();

            Console.WriteLine($"Transaction: {stopwatch.Elapsed}");

            return result;
        }
    }
}
