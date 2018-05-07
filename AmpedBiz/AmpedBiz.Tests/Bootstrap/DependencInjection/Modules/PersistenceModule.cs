using AmpedBiz.Data;
using AmpedBiz.Test.Bootstrap.DependencInjection.Modules.Configurations.Database;
using AmpedBiz.Tests.Context;
using Autofac;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Tests.Bootstrap.DependencInjection.Modules
{
    public class PersistenceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<AuditProvider>()
                .As<IAuditProvider>();

            builder.RegisterType<ValidatorEngine>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<SessionFactoryProvider>()
                .As<ISessionFactoryProvider>()
                .SingleInstance();

            builder.Register(context => context
                    .Resolve<ISessionFactoryProvider>()
                    .WithBatcher(BatcherConfiguration.Configure)
                    .GetSessionFactory()
                )
                .SingleInstance();
        }
    }
}