using AmpedBiz.Core.Common;

namespace AmpedBiz.Core
{
	public interface IHasTenant
    {
        Tenant Tenant { get; set; }
    }
}
