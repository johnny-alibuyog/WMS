using AmpedBiz.Core.Common;
using AmpedBiz.Core.SharedKernel;
using System;

namespace AmpedBiz.Core.Settings
{
    public abstract class SettingType { };

    public abstract class Setting : Entity<Guid, Setting>, IHasTenant
    {
        public virtual Tenant Tenant { get; set; }

        public Setting() : this(default(Guid)) { }

        public Setting(Guid id) : base(id) { }

    }

    public class Setting<TValue> : Setting
        where TValue : SettingType
    {
        public virtual TValue Value { get; set; }

        public Setting() : this(default(Guid)) { }

        public Setting(Guid id) : base(id) { }

        public static Setting<TValue> Default() => new Setting<TValue>() { Value = Activator.CreateInstance<TValue>() };
    }
}