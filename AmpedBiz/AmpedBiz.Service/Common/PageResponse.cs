using System.Collections.Generic;

namespace AmpedBiz.Service.Common
{
    public class PageResponse<T>
    {
        public int? Count { get; set; }

        public IEnumerable<T> Items { get; set; }

        public PageResponse() : this(0, null) { }

        public PageResponse(int count, IEnumerable<T> items)
        {
            this.Count = count;
            this.Items = items;
        }
    }
}
