using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderReceiptValidation : ValidationDef<PurchaseOrderReceipt>
    {
        public PurchaseOrderReceiptValidation()
        {
            Define(x => x.Id);

            Define(x => x.PurchaseOrder)
                .NotNullable();

            Define(x => x.ReceivedBy)
                .NotNullable();

            Define(x => x.ReceivedOn);

            Define(x => x.ExpiresOn)
                .IsInTheFuture();

            Define(x => x.Product)
                .NotNullable();

            Define(x => x.Quantity)
                .NotNullable();
        }
    }
}
