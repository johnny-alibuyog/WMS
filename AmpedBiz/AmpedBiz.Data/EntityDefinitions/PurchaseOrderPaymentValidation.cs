using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderPaymentValidation : ValidationDef<PurchaseOrderPayment>
    {
        public PurchaseOrderPaymentValidation()
        {
            Define(x => x.Id);

            Define(x => x.PurchaseOrder)
                .NotNullable();

            Define(x => x.PaidBy)
                .NotNullable();

            Define(x => x.PaidOn);

            Define(x => x.PaymentType)
                .NotNullable();

            Define(x => x.Payment)
                .NotNullable();
        }
    }
}
