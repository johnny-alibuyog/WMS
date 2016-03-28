using System.Web.Http;
using AmpedBiz.Service.Host.App_Start;

namespace AmpedBiz.Service.Host
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(DependencyConfig.Register);
            SwaggerConfig.Register();
            GlobalConfiguration.Configure(DataSeederConfig.Register);
        }
    }
}