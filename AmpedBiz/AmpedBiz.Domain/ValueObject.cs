using System;
using System.Collections.Generic;
using System.Reflection;

namespace AmpedBiz.Domain
{
    public abstract class ValueObject<T> : IEquatable<T> where T : ValueObject<T>
    {
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var other = obj as T;

            return Equals(other);
        }

        public override int GetHashCode()
        {
            var fields = GetFields();
            var startValue = 17;
            var multiplier = 59;

            var hashCode = startValue;

            foreach (var field in fields)
            {
                var value = field.GetValue(this);

                if (value != null)
                    hashCode = hashCode * multiplier + value.GetHashCode();
            }

            return hashCode;
        }

        public virtual bool Equals(T other)
        {
            if (other == null)
                return false;

            var type = GetType();
            var otherType = other.GetType();

            if (type != otherType)
                return false;

            var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                object value1 = field.GetValue(other);
                object value2 = field.GetValue(this);

                if (value1 == null)
                {
                    if (value2 != null)
                        return false;
                }
                else if (!value1.Equals(value2))
                    return false;
            }

            return true;
        }

        private IEnumerable<FieldInfo> GetFields()
        {
            var type = GetType();
            var fields = new List<FieldInfo>();

            while (type != typeof(object))
            {
                fields.AddRange(type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public));

                type = type.BaseType;
            }

            return fields;
        }

        public static bool operator ==(ValueObject<T> x, ValueObject<T> y)
        {
            return x.Equals(y);
        }

        public static bool operator !=(ValueObject<T> x, ValueObject<T> y)
        {
            return !(x == y);
        }
    }
}
