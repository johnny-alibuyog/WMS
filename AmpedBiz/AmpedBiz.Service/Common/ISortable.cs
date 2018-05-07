using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Common
{
    public enum Direction
    {
        Ascending,
        Descending
    }

    public interface ISortable
    {
        string Field { get; set; }

        Direction? Direction { get; set; }
    }

    public static class SortExtention
    {
        public static bool IsSortable(this ISortable sort)
        {
            if (string.IsNullOrWhiteSpace(sort.Field))
                return false;

            if (sort.Direction == null)
                return false;

            return true;
        }
    }
}
