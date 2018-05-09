using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core
{
    public interface IHasTenant
    {
        Tenant Tenant { get; set; }
    }
}
