using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class OrderPaymentMapping : ClassMap<OrderPayment>
    {
        public OrderPaymentMapping()
        {
            Id(x => x.Id)
                .GeneratedBy.GuidComb();

            References(x => x.Order);

            References(x => x.PaidTo);

            Map(x => x.PaidOn);

            References(x => x.PaymentType);

            Component(x => x.Payment,
                MoneyMapping.Map("Payment_", nameof(OrderPayment)));
        }
    }
}