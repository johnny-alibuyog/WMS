using AmpedBiz.Core.Users;
using System;

namespace AmpedBiz.Core.SharedKernel
{
	public interface IAuditable 
    {
        DateTime? CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }

        User CreatedBy { get; set; }

        User ModifiedBy { get; set; }
    }
}
