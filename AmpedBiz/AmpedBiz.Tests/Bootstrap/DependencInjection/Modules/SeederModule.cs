using AmpedBiz.Data.Seeders;
using Autofac;
using System;
using System.Linq;

namespace AmpedBiz.Tests.Bootstrap.DependencInjection.Modules
{
    public class SeederModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            builder
                .RegisterAssemblyTypes(assemblies)
                .Where(type => type.IsAssignableTo<ISeeder>())
                .AsImplementedInterfaces()
                .AsSelf();

            builder.RegisterType<Runner>().AsSelf();

            //builder
            //    .RegisterAssemblyTypes(assemblies)
            //    .Where(x => x.GetInterfaces().Any(o => 
            //        o.IsAssignableFrom(typeof(IDefaultDataSeeder)) ||
            //        o.IsAssignableFrom(typeof(IDummyDataSeeder))
            //    ))
            //    .AsImplementedInterfaces()
            //    .InstancePerDependency();
        }
    }
}
