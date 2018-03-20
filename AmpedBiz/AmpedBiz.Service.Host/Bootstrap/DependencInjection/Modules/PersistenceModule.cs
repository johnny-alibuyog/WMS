using AmpedBiz.Data;
//using AmpedBiz.Data.Seeder.Seeders;
using AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules.Configurations.Database;
using AmpedBiz.Service.Host.Bootstrap.Providers;
using Autofac;
using NHibernate.Validator.Engine;
using System;

namespace AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules
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