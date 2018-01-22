using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Tests.Bootstrap
{
    public static class Ioc
    {
        public static IContainer Container = Instantiate();

        private static IContainer Instantiate()
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            return builder.Build();

            //var builder = new ContainerBuilder();

            //// Register individual components
            //builder.RegisterInstance(new TaskRepository())
            //       .As<ITaskRepository>();
            //builder.RegisterType<TaskController>();
            //builder.Register(c => new LogManager(DateTime.Now))
            //       .As<ILogger>();

            //// Scan an assembly for components
            //builder.RegisterAssemblyTypes(myAssembly)
            //       .Where(t => t.Name.EndsWith("Repository"))
            //       .AsImplementedInterfaces();

            //var container = builder.Build();
        }
    }
}
