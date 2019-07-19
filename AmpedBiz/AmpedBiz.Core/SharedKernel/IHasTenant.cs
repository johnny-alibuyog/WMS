using AmpedBiz.Core.Common;

namespace AmpedBiz.Core.SharedKernel
{
	public interface IHasTenant
    {
        Tenant Tenant { get; set; }
    }
}
