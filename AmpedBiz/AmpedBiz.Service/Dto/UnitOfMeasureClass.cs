using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Dto
{
    public class UnitOfMeasureClass
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }

        public virtual List<UnitOfMeasure> Units { get; set; }
    }

    public class UnitOfMeasureClassPageItem
    {
        public virtual string Id { get; set; }

        public virtual string Name { get; set; }
    }
}
