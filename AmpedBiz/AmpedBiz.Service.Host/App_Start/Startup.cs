using System.Web.Http;
using AmpedBiz.Service.Host.App_Start;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(AmpedBiz.Service.Host.Startup))]

namespace AmpedBiz.Service.Host
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            DependencyConfig.Register(app, GlobalConfiguration.Configuration);
            SwaggerConfig.Register();
            DataSeederConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}