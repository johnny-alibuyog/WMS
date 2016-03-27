﻿using System;

namespace AmpedBiz.Core.Entities
{
    public class OrderDetail : Entity<OrderDetail, Guid>
    {
        public OrderDetail()
        {
        }

        public virtual Tenant Tenant
        {
            get;
            set;
        }

        public virtual double Quantity
        {
            get;
            set;
        }

        public virtual Money UnitPrice
        {
            get;
            set;
        }

        public virtual double? Discount
        {
            get;
            set;
        }

        public virtual Money ExtendedPrice
        {
            get;
            set;
        }

        public virtual OrderDetailsStatus Status
        {
            get;
            set;
        }

        public virtual bool InsufficientInventory
        {
            get;
            set;
        }

        public virtual bool IsProductAllocated
        {
            get;
            set;
        }

        public virtual bool IsInvoiced
        {
            get;
            set;
        }

        public virtual bool IsShipped
        {
            get;
            set;
        }

        public virtual bool IsBackOrdered
        {
            get;
            set;
        }

        public virtual Product Product
        {
            get;
            set;
        }

        public virtual Order Order
        {
            get;
            set;
        }
    }
}