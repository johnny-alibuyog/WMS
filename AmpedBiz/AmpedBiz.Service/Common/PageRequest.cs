using System;
using System.Collections.Generic;
using AmpedBiz.Common.Extentions;
using System.ComponentModel;

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

            if (value.IsNullOrDefault() && !(value is bool))
                return;

            if (value is string && string.IsNullOrWhiteSpace(value as string))
                return;

            Func<object, TValue> ToType = (val) =>
            {
                if (typeof(TValue).IsEnum)
                    return EnumExtention.As<TValue>(val);

                return (TValue)TypeDescriptor.GetConverter(typeof(TValue))
                    .ConvertFromInvariantString(val.ToString());

                //return (TValue)val;
            }; 

            action(ToType(value));
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

        public virtual bool IsPaged()
        {
            if (this.Size <= 0)
                return false;

            if (this.Offset <= 0)
                return false;

            return true;
        }

        public virtual void RetrieveAll(int count)
        {
            this.Offset = 1;
            this.Size = count;
        }
    }
}
