using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public class Measure
    {
        public decimal Value { get; set; }

        public UnitOfMeasure Unit { get; set; }
    }
}
