using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace Utilities.Reflection
{
    /// <summary>
    /// Contains methods to make reflection of provided type more convenient.
    /// </summary>
    /// <typeparam name="T">Type under reflection</typeparam>
    [UsedImplicitly]
    public static class ReflectionHelper<T>
    {
        /// <summary>
        /// Provides a <see cref="PropertyInfo"/> of the Property pointed to by the <paramref name="propertySelector"/>.
        /// </summary>
        /// <typeparam name="TProperty">Implied from <paramref name="propertySelector"/></typeparam>
        /// <param name="propertySelector">Expression in the form of <example>obj=>obj.Property</example></param>
        /// <returns><see cref="PropertyInfo"/> of the property pointed to by <paramref name="propertySelector"/></returns>
        /// <exception cref="ArgumentException">The provided expression doesn't point to a property! Actual type: </exception>
        /// <exception cref="ArgumentNullException"><paramref name="propertySelector"/> is <see langword="null" />.</exception>
        /// <remarks>
        /// This method allows to easily obtain PropertyInfo of particular classes property, without long reflection method chain or relying on string-held names
        /// </remarks>
        [Pure]
        [NotNull]
        [UsedImplicitly]
        public static PropertyInfo GetPropertyInfoFrom<TProperty>(
            [NotNull] Expression<Func<T, TProperty>> propertySelector)
        {
            if (propertySelector == null) throw new ArgumentNullException(nameof(propertySelector));
            if (propertySelector.Body == null)
                throw new ArgumentNullException(nameof(propertySelector), "Body property is null");

            var body = propertySelector.Body as MemberExpression;
            if (body?.Member != null)
            {
                return (PropertyInfo) body.Member;
            }
            throw new ArgumentException(
                $"The provided expression doesn't point to a property! Actual type: {propertySelector.Body.GetType()}");
        }
    }

    /// <summary>
    /// Provides methods that make work with reflection more convenient.
    /// </summary>
    public static class ReflectionHelper
    {
        /// <summary>
        /// Run-time equivalent for default(<paramref name="type"/>)
        /// </summary>
        /// <param name="type">Type to get default for</param>
        /// <returns>Equivalent of default(type).</returns>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null" />.</exception>
        /// <exception cref="TargetInvocationException">The constructor being called throws an exception. </exception>
        /// <exception cref="MethodAccessException">NoteIn the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MemberAccessException" />, instead.The caller does not have permission to call this constructor. </exception>
        /// <exception cref="MemberAccessException">Cannot create an instance of an abstract class, or this member was invoked with a late-binding mechanism. </exception>
#pragma warning disable 1574
        /// <exception cref="InvalidComObjectException">The COM type was not obtained through <see cref="Type.GetTypeFromProgID" /> or <see cref="Type.GetTypeFromCLSID" />. </exception>
#pragma warning restore 1574
        /// <exception cref="MissingMethodException">NoteIn the .NET for Windows Store apps or the Portable Class Library, catch the base class exception, <see cref="T:System.MissingMemberException" />, instead.No matching public constructor was found. </exception>
        /// <exception cref="COMException"><paramref name="type" /> is a COM object but the class identifier used to obtain the type is invalid, or the identified class is not registered. </exception>
        /// <exception cref="TypeLoadException"><paramref name="type" /> is not a valid type. </exception>
        /// <exception cref="NotSupportedException"><paramref name="type" /> cannot be a <see cref="T:System.Reflection.Emit.TypeBuilder" />.-or- Creation of <see cref="T:System.TypedReference" />, <see cref="T:System.ArgIterator" />, <see cref="T:System.Void" />, and <see cref="T:System.RuntimeArgumentHandle" /> types, or arrays of those types, is not supported.-or-The assembly that contains <paramref name="type" /> is a dynamic assembly that was created with <see cref="F:System.Reflection.Emit.AssemblyBuilderAccess.Save" />. </exception>
        /// <exception cref="ArgumentException"><paramref name="type" /> is not a RuntimeType. -or-<paramref name="type" /> is an open generic type (that is, the <see cref="P:System.Type.ContainsGenericParameters" /> property returns true).</exception>
        [Pure]
        [CanBeNull]
        [UsedImplicitly]
        public static object GetDefault([NotNull] Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));

            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }
    }
}