using MediatR.Pipeline;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AmpedBiz.Service
{
    public class RequestPreProcessorBase<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly TextWriter _writer;

        public RequestPreProcessorBase(TextWriter writer)
        {
            _writer = writer;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            return _writer.WriteLineAsync("- Starting Up");
        }
    }
}
