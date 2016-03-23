using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Common.Exceptions
{
    public class InvalidField
    {
        public virtual string Entity { get; set; }
        public virtual string Property { get; set; }
        public virtual string Message { get; set; }
        public virtual object Value { get; set; }
    }
}
