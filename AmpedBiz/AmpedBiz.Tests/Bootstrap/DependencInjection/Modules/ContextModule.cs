using AmpedBiz.Data.Context;
using AmpedBiz.Tests.Context;
using Autofac;

namespace AmpedBiz.Tests.Bootstrap.DependencInjection.Modules
{
    public class ContextModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ContextProvider>()
                .As<IContextProvider>()
                .SingleInstance();

            builder.Register(context => context
                    .Resolve<IContextProvider>()
                    .Build()
                )
                .As<IContext>()
                .InstancePerLifetimeScope();
        }
    }
}