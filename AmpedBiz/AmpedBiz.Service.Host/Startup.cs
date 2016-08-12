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
            var config = new HttpConfiguration();

            WebApiConfig.Register(config);
            DependencyConfig.Register(app, config);
            SwaggerConfig.Register();
            DataSeederConfig.Register(config);
        }
    }
}
