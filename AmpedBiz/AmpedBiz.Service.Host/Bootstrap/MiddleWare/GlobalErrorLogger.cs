using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace AmpedBiz.Service.Host.Bootstrap.MiddleWare
{
    public class GlobalErrorLogger : IExceptionLogger
    {
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}