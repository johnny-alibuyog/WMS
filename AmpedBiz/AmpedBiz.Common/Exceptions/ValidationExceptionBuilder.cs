using System.Text;

namespace AmpedBiz.Common.Exceptions
{
    public class ValidationExceptionBuilder : IExceptionBuilder<ValidationException>
    {
        private static readonly StringBuilder _builder = new StringBuilder();
        private static readonly string _format = "{0}.{1} has an invalid value of {2}.";

        public ValidationException Build(params InvalidField[] invalidFields)
        {
            _builder.Clear();

            foreach (var item in invalidFields)
            {
                var message = string.IsNullOrWhiteSpace(item.Message)
                    ? string.Format(_format, item.Entity, item.Property, item.Value)
                    : item.Message;

                _builder.AppendLine(message);
            }

            return new ValidationException(_builder.ToString());
        }
    }
}