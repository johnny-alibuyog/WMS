using AmpedBiz.Data.Context;
using AmpedBiz.Service.Host.Context;
using Autofac;

namespace AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules
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

            //builder.RegisterType<RequestContext>()
            //    .As<IContext>()
            //    .InstancePerLifetimeScope();

            //builder.RegisterType<DefaultContext>()
            //    .AsSelf()
            //    .SingleInstance();
        }
    }
}