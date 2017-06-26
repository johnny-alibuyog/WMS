using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace AmpedBiz.Service.Host.Plugins.MiddleWare
{
    public class GlobalErrorLogger : IExceptionLogger
    {
        public Task LogAsync(ExceptionLoggerContext context, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}