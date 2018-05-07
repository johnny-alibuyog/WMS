using AmpedBiz.Core.Entities;
using AmpedBiz.Data.Context;
using Autofac;
using System.Reflection;

namespace AmpedBiz.Tests.Bootstrap
{
    public static class Ioc
    {
        public static IContainer Container = BuildContainer((TenantId)Tenant.Default.Id);

        private static IContainer BuildContainer(TenantId tenantId)
        {
            var builder = new ContainerBuilder();

            builder.RegisterInstance(tenantId);
            builder.RegisterInstance((BranchId)Branch.Default.Id);
            builder.RegisterInstance((UserId)User.Admin.Id);
            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

            return builder.Build();
        }
    }
}
