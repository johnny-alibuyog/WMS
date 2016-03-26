using System;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core
{
    public interface IAuditable 
    {
        DateTimeOffset CreatedOn { get; set; }
        User CreatedBy { get; set; }
        DateTimeOffset ModifiedOn { get; set; }
        User ModifiedBy { get; set; }
    }
}
