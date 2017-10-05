﻿using System;
using AmpedBiz.Core.Entities;

namespace AmpedBiz.Core
{
    public interface IAuditable 
    {
        DateTime? CreatedOn { get; set; }

        DateTime? ModifiedOn { get; set; }

        User CreatedBy { get; set; }

        User ModifiedBy { get; set; }
    }
}
