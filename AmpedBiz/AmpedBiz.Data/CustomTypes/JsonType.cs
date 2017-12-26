using System;
using System.Data;
using System.Data.Common;
using Newtonsoft.Json;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;
using Newtonsoft.Json.Serialization;

namespace AmpedBiz.Data.CustomTypes
{
    [Serializable]
    public class JsonType<T> : IUserType where T : class
    {
        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
        };

        public new bool Equals(object x, object y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            var xdocX = JsonConvert.SerializeObject(x, this._settings);
            var xdocY = JsonConvert.SerializeObject(y, this._settings);

            return xdocY == xdocX;
        }

        public int GetHashCode(object x)
        {
            if (x == null)
                return 0;

            return x.GetHashCode();
        }

        public object NullSafeGet(IDataReader rs, string[] names, object owner)
        {
            if (names.Length != 1)
                throw new InvalidOperationException("Only expecting one column...");

            var val = rs[names[0]] as string;

            if (val != null && !string.IsNullOrWhiteSpace(val))
            {
                return JsonConvert.DeserializeObject<T>(val, this._settings);
            }

            return null;
        }

        public void NullSafeSet(IDbCommand cmd, object value, int index)
        {
            var parameter = (DbParameter)cmd.Parameters[index];

            if (value == null)
            {
                parameter.Value = DBNull.Value;
            }
            else
            {
                parameter.Value = JsonConvert.SerializeObject(value, this._settings);
            }
        }

        public object DeepCopy(object value)
        {
            if (value == null)
                return null;

            //Serialized and Deserialized using json.net so that I don't
            //have to mark the class as serializable. Most likely slower
            //but only done for convenience. 

            var serialized = JsonConvert.SerializeObject(value, this._settings);

            return JsonConvert.DeserializeObject<T>(serialized, this._settings);
        }

        public object Replace(object original, object target, object owner)
        {
            return original;
        }

        public object Assemble(object cached, object owner)
        {
            var str = cached as string;

            if (string.IsNullOrWhiteSpace(str))
                return null;

            return JsonConvert.DeserializeObject<T>(str, this._settings);
        }

        public object Disassemble(object value)
        {
            if (value == null)
                return null;

            return JsonConvert.SerializeObject(value, this._settings);
        }

        public SqlType[] SqlTypes
        {
            get { return new SqlType[] { new StringSqlType() }; }
        }

        public Type ReturnedType
        {
            get { return typeof(T); }
        }

        public bool IsMutable
        {
            get { return true; }
        }
    }
}
