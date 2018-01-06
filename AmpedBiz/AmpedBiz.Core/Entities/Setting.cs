using System;

namespace AmpedBiz.Core.Entities
{
    public interface ISettingType { };

    public abstract class Setting : Entity<Guid, Setting>
    {
        public virtual Tenant Tenant { get; set; }

        public Setting() : this(default(Guid)) { }

        public Setting(Guid id) : base(id) { }

    }

    public class Setting<TValue> : Setting
        where TValue : ISettingType
    {
        public virtual TValue Value { get; set; }

        public Setting() : this(default(Guid)) { }

        public Setting(Guid id) : base(id) { }

        public static Setting<TValue> Default(Tenant tenant) => new Setting<TValue>()
        {
            Tenant = tenant,
            Value = Activator.CreateInstance<TValue>()
        };
    }
}