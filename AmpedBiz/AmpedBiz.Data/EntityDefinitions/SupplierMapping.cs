﻿using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class SupplierMapping : ClassMap<Supplier>
    {
        public SupplierMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);

            Component(x => x.Address);

            Component(x => x.Contact);
        }
    }
}