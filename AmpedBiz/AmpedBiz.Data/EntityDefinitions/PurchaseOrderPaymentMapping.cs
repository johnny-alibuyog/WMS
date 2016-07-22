using AmpedBiz.Core.Entities;
using FluentNHibernate.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderPaymentMapping : ClassMap<PurchaseOrderPayment>
    {
        public PurchaseOrderPaymentMapping()
        {
            Id(x => x.Id)
               .GeneratedBy.GuidComb();

            References(x => x.PurchaseOrder);

            References(x => x.PaidBy);

            Map(x => x.PaidOn);

            References(x => x.PaymentType);

            Component(x => x.Payment, 
                MoneyMapping.Map("Payment_", nameof(PurchaseOrderPayment)));
        }
    }
}
