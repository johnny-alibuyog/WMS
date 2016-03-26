using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PaymentTypeMapping : ClassMap<PaymentType>
    {
        public PaymentTypeMapping()
        {
            Schema("public");

            Id(x => x.Id)
                .GeneratedBy.Assigned();

            Map(x => x.Name);
        }
    }
}
