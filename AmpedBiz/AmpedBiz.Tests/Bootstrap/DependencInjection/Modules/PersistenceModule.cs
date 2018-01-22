using AmpedBiz.Data;
using AmpedBiz.Data.Seeders;
using AmpedBiz.Service.Host.Plugins.DependencInjection.Modules.Configurations.Database;
using AmpedBiz.Tests.Bootstrap.Providers;
using Autofac;
using NHibernate.Validator.Engine;
using System;

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

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(type => type.IsAssignableTo<ISeeder>())
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}