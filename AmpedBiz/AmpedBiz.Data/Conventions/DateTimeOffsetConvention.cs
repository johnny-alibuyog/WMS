using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions.Instances;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace AmpedBiz.Data.Conventions
{
    /// NOTE: This is only Npsql2 which doesn't have any updates as of late

    //public class DateTimeOffsetConvention : IPropertyConvention, IPropertyConventionAcceptance
    //{

    //    public void Accept(IAcceptanceCriteria<IPropertyInspector> criteria)
    //    {
    //        criteria.Expect(x => x.Type == typeof(DateTimeOffset));
    //    }

    //    public void Apply(IPropertyInstance instance)
    //    {
    //        instance.CustomType<DateTimeOffsetValueConverter>();
    //    }
    //}

    //public class DateTimeOffsetValueConverter : IUserType
    //{
    //    public object Assemble(object cached, object owner)
    //    {
    //        return cached;
    //    }

    //    public object DeepCopy(object value)
    //    {
    //        if (value == null) return null;

    //        var dtoffset = (DateTimeOffset)value;
    //        return new DateTimeOffset(dtoffset.Ticks, dtoffset.Offset);
    //    }

    //    public object Disassemble(object value)
    //    {
    //        return value;
    //    }

    //    public new bool Equals(object x, object y)
    //    {
    //        if (x == null)
    //            return false;
    //        else
    //            return x.Equals(y);
    //    }

    //    public int GetHashCode(object x)
    //    {
    //        return x.GetHashCode();
    //    }

    //    public bool IsMutable
    //    {
    //        get { return false; }
    //    }

    //    public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
    //    {

    //        var ordinal = rs.GetOrdinal(names[0]);

    //        if (rs.IsDBNull(ordinal))
    //        {
    //            return null;
    //        }
    //        else 
    //        {
    //            NpgsqlTimeStampTZ timestamptz = (NpgsqlTimeStampTZ)rs.GetValue(ordinal);
    //            return new DateTimeOffset(timestamptz.Ticks, new TimeSpan(timestamptz.TimeZone.Hours, timestamptz.TimeZone.Minutes, timestamptz.TimeZone.Seconds));
    //        }
    //    }

    //    public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
    //    {
    //        if (value == null)
    //        {
    //            (cmd.Parameters[index] as IDataParameter).Value = DBNull.Value;
    //            return;
    //        }
    //        var offsetDate = (DateTimeOffset)value;
    //        var zone = new NpgsqlTimeZone(offsetDate.Offset);
    //        var timestamptz = new NpgsqlTimeStampTZ(offsetDate.Year, offsetDate.Month, offsetDate.Day, offsetDate.Hour, offsetDate.Minute, offsetDate.Second, zone);
    //        (cmd.Parameters[index] as IDataParameter).Value = timestamptz;
    //    }

    //    public object Replace(object original, object target, object owner)
    //    {
    //        return original;
    //    }

    //    public Type ReturnedType
    //    {
    //        get { return typeof(DateTimeOffset); }
    //    }

    //    public SqlType[] SqlTypes
    //    {
    //        get { return new SqlType[] { new SqlType(DbType.DateTimeOffset) }; }
    //    }
    //}

    //public class TimestampValueConverter : IUserType
    //{
    //    public object Assemble(object cached, object owner)
    //    {
    //        return cached;
    //    }

    //    public object DeepCopy(object value)
    //    {
    //        if (value == null) return null;

    //        var dtoffset = (DateTime)value;
    //        return new DateTime(dtoffset.Ticks);
    //    }

    //    public object Disassemble(object value)
    //    {
    //        return value;
    //    }

    //    public new bool Equals(object x, object y)
    //    {
    //        if (x == null)
    //            return false;
    //        else
    //            return x.Equals(y);
    //    }

    //    public int GetHashCode(object x)
    //    {
    //        return x.GetHashCode();
    //    }

    //    public bool IsMutable
    //    {
    //        get { return false; }
    //    }

    //    public object NullSafeGet(System.Data.IDataReader rs, string[] names, object owner)
    //    {

    //        var ordinal = rs.GetOrdinal(names[0]);

    //        if (rs.IsDBNull(ordinal))
    //        {
    //            return null;
    //        }
    //        else {
    //            NpgsqlTimeStamp timestamptz = (NpgsqlTimeStamp)rs.GetValue(ordinal);
    //            return new DateTime(timestamptz.Ticks, DateTimeKind.Local);
    //        }
    //    }

    //    public void NullSafeSet(System.Data.IDbCommand cmd, object value, int index)
    //    {
    //        if (value == null)
    //        {
    //            (cmd.Parameters[index] as IDataParameter).Value = DBNull.Value;
    //            return;
    //        }
    //        var offsetDate = (DateTime)value;
    //        var timestamptz = new NpgsqlTimeStamp(offsetDate.Year, offsetDate.Month, offsetDate.Day, offsetDate.Hour, offsetDate.Minute, offsetDate.Second);
    //        (cmd.Parameters[index] as IDataParameter).Value = timestamptz;
    //    }

    //    public object Replace(object original, object target, object owner)
    //    {
    //        return original;
    //    }

    //    public Type ReturnedType
    //    {
    //        get { return typeof(DateTime); }
    //    }

    //    public SqlType[] SqlTypes
    //    {
    //        get { return new SqlType[] { new SqlType(DbType.DateTime) }; }
    //    }
    //}
}
