namespace AmpedBiz.Core.Settings
{
    public class UserSetting : SettingType
    {
        public virtual string DefaultPassword { get; set; } = "pass@123";
    }
}
