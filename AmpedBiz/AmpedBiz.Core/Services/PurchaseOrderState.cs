using AmpedBiz.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Core.Services
{
    public abstract class PurchaseOrderState : IState<PurchaseOrderStatus>
    {
        public PurchaseOrderStatus Status
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public void Next()
        {
            throw new NotImplementedException();
        }

        public void Previous()
        {
            throw new NotImplementedException();
        }
    }
}
