using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class BranchMapping : ClassMap<Branch>
    {
        public BranchMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Name);

            Map(x => x.Description);

            Map(x => x.TaxpayerIdentificationNumber);

            Component(x => x.Contact);

            Component(x => x.Address);
        }
    }
}
