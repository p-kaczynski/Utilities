using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
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

        /// <summary>
        /// Checkes whether <paramref name="type"/> implements <typeparamref name="TInterface"/>
        /// </summary>
        /// <typeparam name="TInterface"></typeparam>
        /// <param name="type">Type to inspect</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Provided type is not an interface</exception>
        /// <exception cref="ArgumentNullException"><paramref name=""/> is <see langword="null" />.</exception>
        /// <exception cref="TargetInvocationException">A static initializer is invoked and throws an exception. </exception>
        [Pure]
        [UsedImplicitly]
        public static bool Implements<TInterface>([NotNull] this Type type)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (!typeof(TInterface).IsInterface)
                throw new ArgumentException($"Provided type {typeof(TInterface).FullName} is not an interface", nameof(TInterface));

            return type.GetInterfaces().Any(i => i == typeof(TInterface));
        }

        /// <summary>
        /// Checkes whether <paramref name="type"/> implements <paramref name="interfaceType"/>
        /// </summary>
        /// <param name="type">Type to inspect</param>
        /// <param name="interfaceType">Interface type to look for.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">Provided type is not an interface</exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="interfaceType"/> is <see langword="null" />.</exception>
        /// <exception cref="TargetInvocationException">A static initializer is invoked and throws an exception. </exception>
        [Pure]
        [UsedImplicitly]
        public static bool Implements([NotNull] this Type type, [NotNull] Type interfaceType)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (interfaceType == null) throw new ArgumentNullException(nameof(interfaceType));

            if (!interfaceType.IsInterface)
                throw new ArgumentException($"Provided type {interfaceType.FullName} is not an interface", nameof(interfaceType));

            // Handle generic interface definition case
            if (interfaceType.IsGenericTypeDefinition)
                return
                    type.GetInterfaces()
                        .Where(i => i.IsGenericType)
                        .Select(i => i.GetGenericTypeDefinition())
                        .Any(i => i == interfaceType);

            return type.GetInterfaces().Any(i => i == interfaceType);
        }

        /// <summary>
        /// Determines whether <paramref name="type"/> is a generic enumerable of a type, that can be assigned to <typeparamref name="TItem"/>
        /// </summary>
        /// <typeparam name="TItem">Type to which elements of the provided enumerable type must be assignable</typeparam>
        /// <param name="type">Type to inspect</param>
        /// <returns><c>true</c> if <paramref name="type"/> implements <see cref="IEnumerable{T}"/>, where T is assiagnable to <typeparamref name="TItem"/>, otherwise <c>false</c></returns>
        /// <exception cref="TargetInvocationException">A static initializer is invoked and throws an exception. </exception>
        /// <exception cref="ArgumentNullException"><paramref name="type"/> is <see langword="null" />.</exception>
        [Pure]
        [UsedImplicitly]
        public static bool IsGenericEnumerableOf<TItem>([NotNull] this Type type)
        {
            //return ReflectionHelper.GetGenericEnumerableItemType(type) is TItem;
            var enumerableType = ReflectionHelper.GetGenericEnumerableItemType(type);
            return typeof(TItem).IsAssignableFrom(enumerableType);
        }

        /// <summary>
        /// Checks whether the property is virtual, i.e. whether both getter and setter are virtual methods
        /// </summary>
        /// <param name="propertyInfo">PropertyInfo to inspect</param>
        /// <returns><c>true</c> if <paramref name="propertyInfo"/>'s getter and setter are virtual, otherwise <c>false</c></returns>
        /// <exception cref="ArgumentNullException"><paramref name="propertyInfo"/> is <see langword="null" />.</exception>
        /// <exception cref="SecurityException">The requested method is non-public and the caller does not have <see cref="T:System.Security.Permissions.ReflectionPermission" /> to reflect on this non-public method. </exception>
        [Pure]
        [UsedImplicitly]
        public static bool IsVirtual([NotNull] this PropertyInfo propertyInfo)
        {
            if (propertyInfo == null) throw new ArgumentNullException(nameof(propertyInfo));
            
            // bool? == true means: not null and not false
            return propertyInfo.GetGetMethod(true)?.IsVirtual == true && propertyInfo.GetSetMethod(true)?.IsVirtual == true;
        }
    }
}