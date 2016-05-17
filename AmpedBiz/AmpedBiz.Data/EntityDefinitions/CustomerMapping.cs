using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class CustomerMapping : ClassMap<Customer>
    {
        public CustomerMapping()
        {
            Id(x => x.Id).GeneratedBy.Assigned();

            Map(x => x.Name);

            References(x => x.PricingScheme);

            Component(x => x.CreditLimit, MoneyMapping.Map("CreditLimit_", nameof(Customer)));

            Component(x => x.OfficeAddress, AddressMapping.Map("Office_"));

            Component(x => x.BillingAddress, AddressMapping.Map("Billing_"));

            Component(x => x.Contact);

        }
    }
}