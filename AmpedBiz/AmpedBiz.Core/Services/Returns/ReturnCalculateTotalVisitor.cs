using AmpedBiz.Core.Entities;
using System.Linq;

namespace AmpedBiz.Core.Services.Returns
{
    public class ReturnCalculateTotalVisitor : IVisitor<Return>
    {
        public virtual void Visit(Return target)
        {
            if (target.Items.Any())
            {
                target.Total = target.Items
                    .Where(x =>
                        x.TotalPrice != null &&
                        x.TotalPrice.Currency != null
                    )
                    .Sum(x => x.TotalPrice);
            }
        }
    }
}
