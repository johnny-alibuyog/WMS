﻿using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Common
{
    public class Person : ValueObject<Person>
    {
        public virtual string FirstName { get; set; }

        public virtual string LastName { get; set; }

        public virtual string MiddleName { get; set; }

        public virtual Nullable<DateTime> BirthDate { get; set; }
    }
}
