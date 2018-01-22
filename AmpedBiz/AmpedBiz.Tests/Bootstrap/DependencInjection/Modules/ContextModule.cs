using AmpedBiz.Data.Context;
using Autofac;

namespace AmpedBiz.Tests.Bootstrap.DependencInjection.Modules
{
    public class ContextModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DefaultContext>()
                .As<IContext>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DefaultContext>()
                .AsSelf()
                .SingleInstance();
        }
    }
}