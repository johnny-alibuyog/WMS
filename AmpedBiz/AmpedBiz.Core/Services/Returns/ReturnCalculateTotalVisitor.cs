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
                target.TotalReturned = target.Items
                    .Where(x =>
                        x.Returned != null &&
                        x.Returned.Currency != null
                    )
                    .Sum(x => x.Returned);
            }
        }
    }
}
