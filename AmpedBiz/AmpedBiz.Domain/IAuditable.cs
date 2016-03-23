﻿using System;

namespace AmpedBiz.Domain
{
    public interface IAuditable 
    {
        DateTimeOffset CreatedOn { get; set; }
        string CreatedBy { get; set; }
        DateTimeOffset ModifiedOn { get; set; }
        string ModifiedBy { get; set; }
    }
}
