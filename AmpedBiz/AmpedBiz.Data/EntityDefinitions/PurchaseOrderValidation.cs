using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Core.Entities;
using NHibernate.Validator.Cfg.Loquacious;

namespace AmpedBiz.Data.EntityDefinitions
{
    public class PurchaseOrderValidation : ValidationDef<PurchaseOrder>
    {
        public PurchaseOrderValidation()
        {
            Define(x => x.Id);

            Define(x => x.CompletedBy);

            Define(x => x.ClosedDate);

            Define(x => x.CreatedBy);

            Define(x => x.CreationDate);

            Define(x => x.ExpectedDate);

            Define(x => x.OrderDate);

            Define(x => x.Payment).
                IsValid();

            Define(x => x.PaymentDate);

            Define(x => x.PaymentType);

            Define(x => x.ShippingFee)
                .IsValid();

            Define(x => x.Status);

            Define(x => x.SubmittedBy);

            Define(x => x.SubmittedDate);

            Define(x => x.SubTotal)
                .IsValid();

            Define(x => x.Supplier);

            Define(x => x.Tax)
                .IsValid();

            Define(x => x.Total)
                .IsValid();
        }
    }
}
