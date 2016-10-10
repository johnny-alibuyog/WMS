using NHibernate.Validator.Engine;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AmpedBiz.Data.Configurations
{
    public class ConventionMessageInterpolator : IMessageInterpolator
    {
        private const string EntityValidatorConvention = "{{validator.{0}}}";
        private const string EntityPropertyValidatorConvention = "{{validator.{0}.{1}}}";
        private const string EntityNameConvention = "{{friendly.class.{0}}}";
        private const string PropertyNameConvention = "{{friendly.property.{0}}}";
        private const string PropertyValueTagSubstitutor = "${{{0}{1}}}";
        private static readonly int PropertyValueTagLength = "PropertyValue".Length;

        private readonly Regex SubstitutionExpression = new Regex(
            pattern: @"\[EntityName\]|\[PropertyName\]|(\[PropertyValue([.][A-Za-z_][A-Za-z_0-9]*)*\])",
            options: RegexOptions.Compiled
        );

        #region IMessageInterpolator Members

        public string Interpolate(InterpolationInfo info)
        {
            var result = info.Message;

            if (string.IsNullOrEmpty(result))
                result = CreateDefaultMessage(info);

            do
            {
                info.Message = Replace(result, info.Entity, info.PropertyName);
                result = info.DefaultInterpolator.Interpolate(info);
            }
            while (!Equals(result, info.Message));

            return result;
        }

        #endregion

        #region Routine Helpers

        private string CreateDefaultMessage(InterpolationInfo info)
        {
            return string.IsNullOrEmpty(info.PropertyName)
                ? string.Format(EntityValidatorConvention, GetEntityValidatorName(info))
                : string.Format(EntityPropertyValidatorConvention, info.Entity.Name, info.PropertyName);
        }

        private string GetEntityValidatorName(InterpolationInfo info)
        {
            var entityValidatorName = info.Entity.Name;
            var validatorType = info.Validator.GetType();
            if (validatorType.IsGenericType)
                entityValidatorName = validatorType.GetGenericArguments().First().Name;

            entityValidatorName = CleanValidatorPostfix(entityValidatorName);
            return entityValidatorName;
        }

        private string CleanValidatorPostfix(string entityValidatorName)
        {
            var postfixIndex = entityValidatorName.LastIndexOf("Validator");
            return postfixIndex > 0
                ? entityValidatorName.Substring(0, postfixIndex)
                : entityValidatorName;
        }

        private string CleanEntityPostfix(string entityName)
        {
            var name = entityName;
            var postfixIndex = default(int);

            postfixIndex = name.LastIndexOf("ViewModel");
            if (postfixIndex > 0)
                name = name.Substring(0, postfixIndex);

            postfixIndex = name.LastIndexOf("Entity");
            if (postfixIndex > 0)
                name = name.Substring(0, postfixIndex);

            postfixIndex = name.LastIndexOf("Model");
            if (postfixIndex > 0)
                name = name.Substring(0, postfixIndex);

            return name;
        }

        private string Replace(string originalMessage, Type entity, string propName)
        {
            return SubstitutionExpression.Replace(originalMessage, match =>
            {
                if ("[EntityName]".Equals(match.Value))
                    return CleanEntityPostfix(entity.Name);//return entity.Name;

                if ((!string.IsNullOrEmpty(propName) && "[PropertyName]".Equals(match.Value)))
                    return propName;

                if (!string.IsNullOrEmpty(propName) && match.Value.StartsWith("[PropertyValue"))
                    return string.Format(PropertyValueTagSubstitutor, propName, match.Value.Trim('[', ']').Substring(PropertyValueTagLength));

                return match.Value;
            });
        }

        #endregion
    }
}
