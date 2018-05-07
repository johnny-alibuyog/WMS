using AmpedBiz.Data.Context;
using AmpedBiz.Data.Initializer.Context;
using Autofac;

namespace AmpedBiz.Data.Initializer.Bootstrap.DependencInjection.Modules
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

            //builder.RegisterType<DefaultContext>()
            //    .As<IContext>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<DefaultContext>()
            //    .AsSelf()
            //    .SingleInstance();
        }
    }
}