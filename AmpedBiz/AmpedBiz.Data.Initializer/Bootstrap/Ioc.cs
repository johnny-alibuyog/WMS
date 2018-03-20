using Autofac;
using System.Reflection;

namespace AmpedBiz.Data.Initializer.Bootstrap
{
    public static class Ioc
    {
        public static IContainer Container = Instantiate();

        private static IContainer Instantiate()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            return builder.Build();
        }
    }
}
