using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core.Services.Returns
{
    public abstract class ReturnVisitor : IVisitor<Return>
    {
        public abstract void Visit(Return target);
    }
}
