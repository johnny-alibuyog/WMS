using AmpedBiz.Common.CustomTypes;

namespace AmpedBiz.Service.Dto
{
    public class ProductUnitOfMeasurePrice
    {
        public virtual Lookup<string> Pricing { get; set; }

        public virtual decimal? PriceAmount { get; protected set; }
    }
}
