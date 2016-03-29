using System;
using System.Linq.Expressions;
using System.Reflection;

namespace AmpedBiz.Common.Extentions
{
    public static class ReflectionExtention
    {
        public static MemberInfo GetMemberInfo<T, U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException("Expression is not a member access", "expression");

            return member.Member;
        }

        public static string GetMemberName<T, U>(Expression<Func<T, U>> expression)
        {
            var member = ReflectionExtention.GetMemberInfo(expression);
            return member.Name;
        }
    }
}
