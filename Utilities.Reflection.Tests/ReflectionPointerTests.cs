using Should;
using Xunit;

namespace Utilities.Reflection.Tests
{
    public class ReflectionPointerTests
    {
        public const int Test1 = 1;
        public static string Test2 = "2";
        public static string[] Test3 => new[] {$"ping", "pong"};

        [Fact]
        public void TestConst()
        {
            var pointer = ReflectionPointer<int>.Create(() => ReflectionPointerTests.Test1);
            pointer.Invoke().ShouldBeSameAs(Test1);
        }

        [Fact]
        public void TestStaticField()
        {
            var pointer = ReflectionPointer<string>.Create(() => ReflectionPointerTests.Test2);
            pointer.Invoke().ShouldBeSameAs(Test2);
        }

        [Fact]
        public void TestStaticProperty()
        {
            var pointer = ReflectionPointer<string[]>.Create(() => ReflectionPointerTests.Test3);
            pointer.Invoke().ShouldBeSameAs(Test3);
        }
    }
}