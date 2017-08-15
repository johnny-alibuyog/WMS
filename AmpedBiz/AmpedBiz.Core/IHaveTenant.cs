using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core
{
    public interface IHaveTenant
    {
        Tenant Tenant { get; set; }
    }
}
