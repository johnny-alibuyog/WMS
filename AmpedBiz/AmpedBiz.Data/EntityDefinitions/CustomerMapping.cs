using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CustomerMapping : ClassMap<Customer>
    {
        public CustomerMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            Map(x => x.Code);

            Map(x => x.Name);

            References(x => x.Pricing);

            Component(x => x.Contact);

            Map(x => x.IsActive);

            Component(x => x.CreditLimit, 
                MoneyMapping.Map("CreditLimit_", nameof(Customer)));

            Component(x => x.OfficeAddress, 
                AddressMapping.Map("Office_"));

            Component(x => x.BillingAddress, 
                AddressMapping.Map("Billing_"));
        }
    }
}