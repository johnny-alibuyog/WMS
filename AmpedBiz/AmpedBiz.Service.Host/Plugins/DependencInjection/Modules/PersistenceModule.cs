using System;
using AmpedBiz.Data;
using AmpedBiz.Data.Configurations;
using AmpedBiz.Data.DataInitializer;
using AmpedBiz.Service.Host.Plugins.Providers;
using AmpedBiz.Service.Host.Properties;
using Autofac;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Service.Host.Plugins.DependencInjection.Modules
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

            builder.Register<SessionProvider>(context =>
                    new SessionProvider(
                        validator: context.Resolve<ValidatorEngine>(),
                        auditProvider: context.Resolve<IAuditProvider>(),
                        host: Settings.Default.Host,
                        port: Settings.Default.Port,
                        database: Settings.Default.Database,
                        username: Settings.Default.Username,
                        password: Settings.Default.Password
                    )
                )
                .As<ISessionProvider>()
                .SingleInstance();

            builder.Register(context => context.Resolve<ISessionProvider>().SessionFactory)
                .SingleInstance();

            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
                .Where(type => type.IsAssignableTo<IDataSeeder>())
                .AsImplementedInterfaces()
                .AsSelf();
        }
    }
}