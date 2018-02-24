using AmpedBiz.Service.Host.Bootstrap.MiddleWare;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace AmpedBiz.Service.Host.App_Start
{
    public class MindlewareConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
        }
    }
}