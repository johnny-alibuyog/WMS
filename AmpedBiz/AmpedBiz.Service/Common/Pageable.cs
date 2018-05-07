using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpedBiz.Service.Common
{
    public static class Pageable
    {
        public interface IRequest
        {
            int? Size { get; set; }

            int? Offset { get; set; }
        }

        public interface IResponse<T>
        {
            int? Count { get; set; }

            List<T> Items { get; set; }
        }
    }

    public static class PageableExtention
    {
        public static bool IsPageable(this Pageable.IRequest page)
        {
            if (page.Size <= 0)
                return false;

            if (page.Offset <= 0)
                return false;

            return true;
        }
    }
}
