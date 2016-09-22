using AmpedBiz.Core.Entities;
using System.Linq;

namespace AmpedBiz.Core.Services.Returns
{
    public class ReturnCalculateTotalVisitor : ReturnVisitor
    {
        public override void Visit(Return target)
        {
            if (target.Items.Any())
            {
                var currency = target.Items
                    .Where(x =>
                        x.TotalPrice != null &&
                        x.TotalPrice.Currency != null
                    )
                    .Select(x => x.TotalPrice.Currency)
                    .FirstOrDefault();

                var amount = target.Items
                    .Where(x =>
                        x.TotalPrice != null &&
                        x.TotalPrice.Currency != null
                    )
                    .Sum(x => x.TotalPrice.Amount);

                target.Total = new Money(amount, currency);
            }
        }
    }
}
