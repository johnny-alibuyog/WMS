using AmpedBiz.Data.Context;
using AmpedBiz.Service.Host.Context;
using Autofac;

namespace AmpedBiz.Service.Host.Plugins.DependencInjection.Modules
{
    public class ContextModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequestContext>()
                .As<IContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DefaultContext>()
                .AsSelf()
                .SingleInstance();
        }
    }
}