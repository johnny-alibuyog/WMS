using AmpedBiz.Data.Definitions.Users;
using NHibernate.Cfg;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;
using System.Reflection;

namespace AmpedBiz.Data.Configurations
{
	internal static class ValidatorConfiguration
    {
        public static void Configure(Configuration configuration)
        {
            var validatorEngine = GetValidatorEngine();
            new ValidatorSharedEngineProvider(validatorEngine).UseMe();
            configuration.Initialize(validatorEngine);
        }

        private static ValidatorEngine GetValidatorEngine()
        {
            var configuration = GetConfiguration();
            SessionFactoryProvider.Validator.Configure(configuration);
            return SessionFactoryProvider.Validator;
        }

        private static FluentConfiguration GetConfiguration()
        {
            var configuration = new FluentConfiguration();
            configuration
                .SetMessageInterpolator<ConventionMessageInterpolator>()
                .SetCustomResourceManager("AmpedBiz.Data.Properties.CustomValidatorMessages", Assembly.Load("AmpedBiz.Data"))
                .SetDefaultValidatorMode(ValidatorMode.OverrideExternalWithAttribute)
                .Register(Assembly.Load(typeof(UserDefinition.Mapping).Assembly.FullName).ValidationDefinitions())
                .IntegrateWithNHibernate
                    .ApplyingDDLConstraints()
                    .RegisteringListeners();

            return configuration;
        }
    }
}
