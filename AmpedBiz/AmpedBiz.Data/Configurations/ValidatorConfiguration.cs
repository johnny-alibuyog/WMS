using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AmpedBiz.Data.EntityDefinitions;
using AmpedBiz.Core;
using NHibernate.Cfg;
using NHibernate.Validator.Cfg;
using NHibernate.Validator.Cfg.Loquacious;
using NHibernate.Validator.Engine;

namespace AmpedBiz.Data.Configurations
{
    internal static class ValidatorConfiguration
    {
        public static void Configure(this Configuration configuration)
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
                .Register(Assembly.Load(typeof(UserMapping).Assembly.FullName).ValidationDefinitions())
                .IntegrateWithNHibernate
                    .ApplyingDDLConstraints()
                    .And.RegisteringListeners();

            return configuration;
        }
    }
}
