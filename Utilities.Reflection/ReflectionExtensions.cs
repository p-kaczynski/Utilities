using System;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;

namespace Utilities.Reflection
{
    /// <summary>
    /// Contains extension methods to facilitate using reflection
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks wheter the <typeparamref name="TAttribute"/> is declared on the <paramref name="memberInfo"/>.
        /// </summary>
        /// <typeparam name="TAttribute">Attribute type to look for</typeparam>
        /// <param name="memberInfo">Member to check</param>
        /// <param name="inherit">Whether to accept attributes declared on inherited members</param>
        /// <returns><c>true</c> if attribute is declared, otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException"><paramref name="memberInfo"/> is <see langword="null" />.</exception>
        /// <exception cref="TypeLoadException">A custom attribute type cannot be loaded. </exception>
        /// <exception cref="NotSupportedException"><paramref name="memberInfo" /> is not a constructor, method, property, event, type, or field. </exception>
        [Pure]
        [UsedImplicitly]
        public static bool HasCustomAttribute<TAttribute>([NotNull] this MemberInfo memberInfo, bool inherit = false) where TAttribute : Attribute
        {
            if (memberInfo == null) throw new ArgumentNullException(nameof(memberInfo));
            return memberInfo.GetCustomAttributes<TAttribute>(inherit).Any();
        }

        /// <summary>
        /// Checks wheter provided methodInfo is an getter of any of declaring type properties' getter
        /// </summary>
        /// <param name="methodInfo">Method to check</param>
        /// <param name="includeNonPublic">Whether to include non-public getters</param>
        /// <param name="bindingFlags">Optionally allows narrowing which properties are considered</param>
        /// <returns><c>true</c> if the <paramref name="methodInfo"/> is a getter method, otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException"><paramref name="methodInfo"/> or DeclaringType property is <see langword="null" />.</exception>
        [Pure]
        [UsedImplicitly]
        public static bool IsGetter([NotNull] this MethodInfo methodInfo, bool includeNonPublic = true, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.DeclaringType == null) throw new ArgumentNullException(nameof(methodInfo),"DeclaringType is null");
            return
                methodInfo.DeclaringType.GetProperties(bindingFlags).Any(prop => prop.GetGetMethod(includeNonPublic) == methodInfo);
        }

        /// <summary>
        /// Checks wheter provided methodInfo is an setter of any of declaring type properties' getter
        /// </summary>
        /// <param name="methodInfo">Method to check</param>
        /// <param name="includeNonPublic">Whether to include non-public setters</param>
        /// <param name="bindingFlags">Optionally allows narrowing which properties are considered</param>
        /// <returns><c>true</c> if the <paramref name="methodInfo"/> is a setter method, otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException"><paramref name="methodInfo"/> or DeclaringType property is <see langword="null" />.</exception>
        [Pure]
        [UsedImplicitly]
        public static bool IsSetter([NotNull] this MethodInfo methodInfo, bool includeNonPublic = true, BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        {
            if (methodInfo == null) throw new ArgumentNullException(nameof(methodInfo));
            if (methodInfo.DeclaringType == null) throw new ArgumentNullException(nameof(methodInfo), "DeclaringType is null");

            return
                methodInfo.DeclaringType.GetProperties(bindingFlags).Any(prop => prop.GetSetMethod(includeNonPublic) == methodInfo);
        }
    }
}