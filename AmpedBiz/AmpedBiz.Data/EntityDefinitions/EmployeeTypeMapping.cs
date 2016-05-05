using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class EmployeeTypeMapping : ClassMap<EmployeeType>
    {
        public EmployeeTypeMapping()
        {
            Id(x => x.Id)
                .GeneratedBy
                .Assigned();

            Map(x => x.Name);
        }
    }
}