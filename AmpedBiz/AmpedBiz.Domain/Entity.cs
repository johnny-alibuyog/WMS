﻿namespace AmpedBiz.Domain
{
    public abstract class Entity<TEntity, TId> where TEntity : Entity<TEntity, TId>
    {
        private int? _oldHashCode;

        private bool IsTransient { get { return Equals(this.Id, default(TId)); } }

        public virtual TId Id { get; protected set; }

        public Entity() { }

        public Entity(TId id)
        {
            this.Id = id;
        }

        #region Equality Comparer

        public override bool Equals(object obj)
        {
            var other = obj as TEntity;
            if (other == null)
                return false;

            //to handle the case of comparing two new objects
            if (this.IsTransient && other.IsTransient)
                return ReferenceEquals(other, this);

            return other.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            //This is done se we won't change the hash code
            if (_oldHashCode.HasValue)
            {
                return _oldHashCode.Value;
            }

            //When we are transient, we use the base GetHashCode() and remember it, so an instance can't change its hash code.
            if (this.IsTransient)
            {
                _oldHashCode = base.GetHashCode();
                return _oldHashCode.Value;
            }

            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TEntity, TId> x, Entity<TEntity, TId> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(Entity<TEntity, TId> x, Entity<TEntity, TId> y)
        {
            return !(x == y);
        }

        #endregion
    } 
}
