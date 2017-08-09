using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Should;
using Utilities.Reflection;
using Xunit;

namespace Utilities.Reflection.Tests
{
    public class ReflectionExtensionsTests
    {
        [Fact]
        public void IsGetter_Test()
        {
            typeof(IsGetterIsSetterTest).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.IsGetter()).ShouldEqual(4);
        }

        [Fact]
        public void IsGetter_Test_PublicOnly()
        {
            typeof(IsGetterIsSetterTest).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.IsGetter(false)).ShouldEqual(3);
        }

        [Fact]
        public void IsSetter_Test()
        {
            typeof(IsGetterIsSetterTest).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.IsSetter()).ShouldEqual(3);
        }

        [Fact]
        public void IsSetter_Test_PublicOnly()
        {
            typeof(IsGetterIsSetterTest).GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.IsSetter(false)).ShouldEqual(2);
        }

        [Fact]
        public void HasAttribute_Test()
        {
            typeof(IsGetterIsSetterTest)
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m=>m.HasCustomAttribute<HasAttributeTestAttribute>()).ShouldEqual(5);
        }

        [Fact]
        public void HasAttribute_Test_Inheritance()
        {
            typeof(HasMemberInheritTestSubClass)
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.HasCustomAttribute<HasAttributeTestAttribute>(true)).ShouldEqual(2);
            typeof(HasMemberInheritTestSubClass)
                .GetMembers(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Count(m => m.HasCustomAttribute<HasAttributeTestAttribute>(false)).ShouldEqual(1);
        }

        [Fact]
        public void IsGenericEnumerableOf_Functionality()
        {
            new List<UnderTest>().GetType().IsGenericEnumerableOf<UnderTest>().ShouldBeTrue();
            new List<UnderTestSub1>().GetType().IsGenericEnumerableOf<UnderTest>().ShouldBeTrue();
            new List<UnderTestWithInterface>().GetType().IsGenericEnumerableOf<UnderTest>().ShouldBeFalse();
            new List<UnderTest>().GetType().IsGenericEnumerableOf<IUnderTest>().ShouldBeFalse();
            new List<IUnderTest>().GetType().IsGenericEnumerableOf<IUnderTest>().ShouldBeTrue();
            new List<UnderTestWithInterface>().GetType().IsGenericEnumerableOf<IUnderTest>().ShouldBeTrue();

            typeof(UnderTest[]).IsGenericEnumerableOf<UnderTest>().ShouldBeTrue();
            typeof(UnderTestSub1[]).IsGenericEnumerableOf<UnderTest>().ShouldBeTrue();
            typeof(UnderTestWithInterface[]).IsGenericEnumerableOf<UnderTest>().ShouldBeFalse();
            typeof(UnderTest[]).IsGenericEnumerableOf<IUnderTest>().ShouldBeFalse();
            typeof(IUnderTest[]).IsGenericEnumerableOf<IUnderTest>().ShouldBeTrue();
            typeof(UnderTestWithInterface[]).IsGenericEnumerableOf<IUnderTest>().ShouldBeTrue();
        }

        // TEST CLASSES BELOW

        private class UnderTest { }
        private class UnderTestSub1 : UnderTest{ }
        private interface IUnderTest { }
        private class UnderTestWithInterface : IUnderTest { }

        private class HasAttributeTestAttribute : Attribute
        {
            
        }
        private class IsGetterIsSetterTest
        {
            [HasAttributeTest]
            private string _property;

            [HasAttributeTest]
            public void TestMethod()
            {
                
            }

            public string AnotherMethod()
            {
                return string.Empty;
            }

            [HasAttributeTest]
            public int Test { private get; set; }
            public int Test2 { get; [HasAttributeTest]private set; }

            public string Property
            {
                get { return _property; }
                set { _property = value; }
            }

            public int ExpressionBody => 3*2;

            public bool GetValue() => false;

            [HasAttributeTest]
            private class Sub
            {
                
            }
        }

        private class HasMemberInheritTest
        {
            [HasAttributeTest]
            protected bool Value => false;

            [HasAttributeTest]
            protected virtual bool Value2 => false;
        }

        private class HasMemberInheritTestSubClass : HasMemberInheritTest
        {
            protected override bool Value2 => true;
        }
    }
}