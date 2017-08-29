using AmpedBiz.Common.CustomTypes;
using System;

namespace AmpedBiz.Service.Dto
{
    public class ProductUnitOfMeasurePrice
    {
        public Guid Id { get; set; }

        public virtual Lookup<string> Pricing { get; set; }

        public virtual decimal? PriceAmount { get; set; }
    }
}
