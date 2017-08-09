using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using JetBrains.Annotations;

namespace Utilities.Reflection
{
    [PublicAPI]
    public class ReflectionPointer<TTarget>
    {
        public string DeclaringType { get; set; }
        public string Name { get; set; }
        public string ReflectedType { get; set; }

        public TTarget Invoke()
        {
            var type = Type.GetType(DeclaringType, false, false);
            var member = type?.GetMember(Name)
                .SingleOrDefault(m => m?.ReflectedType?.AssemblyQualifiedName?.Equals(ReflectedType) == true);
            if(member == null)
                throw new ArgumentException($"This instance of {nameof(ReflectionPointer<TTarget>)} points to a member that cannot be found: ({ReflectedType}) [{DeclaringType}] -> {Name}");

            return InvokeDynamic((dynamic) member);
        }

        private TTarget InvokeDynamic(FieldInfo member)
        {
            if(member.IsStatic)
            return (TTarget) member.GetValue(null);
            if (member.IsInitOnly && member.IsLiteral)
                return (TTarget) member.GetRawConstantValue();
            throw new ArgumentException($"This instance of {nameof(ReflectionPointer<TTarget>)} points to a field that's not static or const");
        }

        private TTarget InvokeDynamic(PropertyInfo member)
        {
            if (member.GetMethod.IsStatic)
                return (TTarget) member.GetMethod.Invoke(null, new object[0]);
            throw new ArgumentException($"This instance of {nameof(ReflectionPointer<TTarget>)} points to a non-static property");
        }

        private TTarget InvokeDynamic(MemberInfo member)
        {
            throw new ArgumentException($"The data in this {nameof(ReflectionPointer<TTarget>)} point to a {member.GetType().FullName} that cannot be invoked");
        }

        public static ReflectionPointer<TTarget> Create(Expression<Func<TTarget>> selector)
        {
            if (selector == null) throw new ArgumentNullException(nameof(selector));
            if (selector.Body == null)
                throw new ArgumentNullException(nameof(selector), "Body property is null");

            var body = selector.Body as MemberExpression;
            if (body?.Member == null)
                throw new ArgumentException(
                    $"The provided expression doesn't point to a member! Actual type: {selector.Body.GetType()}");

            var target = body.Member;

            if (target.DeclaringType == null)
                throw new ArgumentException($"The provided expression points to a member with no {nameof(target.DeclaringType)}");
            if (target.ReflectedType== null)
                throw new ArgumentException($"The provided expression points to a member with no {nameof(target.ReflectedType)}");

            EvaluateViability((dynamic) target);

            return new ReflectionPointer<TTarget>
                {
                    DeclaringType = target.DeclaringType.AssemblyQualifiedName,
                    Name = target.Name,
                    ReflectedType = target.ReflectedType.AssemblyQualifiedName
                };
        }

        private static void EvaluateViability(FieldInfo target)
        {
            if (!target.IsStatic || !(target.IsLiteral && target.IsInitOnly))
                throw new ArgumentException("The provided expression points to a field that is not static/const");
        }

        private static void EvaluateViability(PropertyInfo target)
        {
            if (!target.GetMethod.IsStatic)
                throw new ArgumentException("The provided expression points to a property that is not static");
        }

        private static void EvaluateViability(MemberInfo target)
        {
            throw new ArgumentException($"The provided expression points to a member that is not allowed: {target.GetType().FullName}");
        }
    }
}