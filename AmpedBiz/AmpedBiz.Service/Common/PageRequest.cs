using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Common.Extentions;

namespace AmpedBiz.Service.Common
{
    public class PageRequest
    {
        public virtual Filter Filter { get; set; }

        public virtual Sorter Sorter { get; set; }

        public virtual Pager Pager { get; set; }

        public PageRequest()
        {
            this.Filter = new Filter();
            this.Sorter = new Sorter();
            this.Pager = new Pager()
            {
                Offset = 1,
                Size = 10
            };
        }
    }

    public enum SortDirection
    {
        None = 0,
        Ascending = 1,
        Descending = 2
    }

    public class Filter : Dictionary<string, object>
    {
        public void Compose<TValue>(string field, Action<TValue> action)
        {
            if (!this.ContainsKey(field))
                return;

            var value = this[field];
            if (value.IsNullOrDefault())
                return;

            action((TValue)value);
        }
    }

    public class Sorter : Dictionary<string, SortDirection>
    {
        public void Compose(string field, Action<SortDirection> action)
        {
            if (!this.ContainsKey(field))
                return;

            var value = this[field];
            if (value.IsNullOrDefault())
                return;

            action(value);
        }
    }

    public class Pager
    {
        public virtual int Size { get; set; }

        public virtual int Offset { get; set; }

        public virtual int SkipCount { get { return (this.Offset - 1) * this.Size; } }

        public Pager()
        {
            this.Size = 10;
            this.Offset = 1;
        }

        public virtual bool IsPageable()
        {
            if (this.Size <= 0)
                return false;

            if (this.Offset <= 0)
                return false;

            return true;
        }
    }
}
