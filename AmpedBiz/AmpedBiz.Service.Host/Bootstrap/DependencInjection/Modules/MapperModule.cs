﻿using AmpedBiz.Service.Dto.Mappers;
using Autofac;

namespace AmpedBiz.Service.Host.Bootstrap.DependencInjection.Modules
{
    public class MapperModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<Mapper>()
                .As<IMapper>();
        }
    }
}