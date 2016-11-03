using System;
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