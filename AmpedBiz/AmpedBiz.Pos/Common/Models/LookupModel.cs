using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace AmpedBiz.Pos.Common.Models
{
    public abstract class LookupModel<TId> : ReactiveObject, IComparable<LookupModel<TId>>, IEquatable<LookupModel<TId>>
    {
        [Reactive] public TId Id { get; private set; }

        [Reactive] public string Name { get; private set; }

        protected LookupModel(TId id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public override string ToString() => this.Name;

        public override int GetHashCode()
        {
            var hashCode = 17;
            hashCode = hashCode * 59 + this.Id.GetHashCode();
            return hashCode;
        }

        public int CompareTo(LookupModel<TId> other)
        {
            return string.Compare(this.Name, other?.Name);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as LookupModel<TId>);
        }

        public bool Equals(LookupModel<TId> other)
        {
            if (other == null)
                return false;

            return this.Id.Equals(other.Id);
        }
    }
}
