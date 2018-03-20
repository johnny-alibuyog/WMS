using AmpedBiz.Data.Initializer.Bootstrap.DependencInjection.Modules.Configurations.Database;
using AmpedBiz.Data.Initializer.Bootstrap.Providers;
using Autofac;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Data.Initializer.Bootstrap.DependencInjection.Modules
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