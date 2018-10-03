using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;

namespace AmpedBiz.Pos.Common.Models
{
    public abstract class Lookup<TId> : ReactiveObject, IComparable<Lookup<TId>>, IEquatable<Lookup<TId>>
    {
        [Reactive] public TId Id { get; private set; }

        [Reactive] public string Name { get; private set; }

        protected Lookup(TId id, string name)
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

        public int CompareTo(Lookup<TId> other)
        {
            return string.Compare(this.Name, other?.Name);
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as Lookup<TId>);
        }

        public bool Equals(Lookup<TId> other)
        {
            if (other == null)
                return false;

            return this.Id.Equals(other.Id);
        }
    }
}
