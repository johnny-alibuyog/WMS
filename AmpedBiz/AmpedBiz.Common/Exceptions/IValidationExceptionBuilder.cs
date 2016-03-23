using System;

namespace AmpedBiz.Common.Exceptions
{
    public interface IExceptionBuilder<TException> where TException : Exception
    {
        TException Build(params InvalidField[] invalidFields);
    }
}
