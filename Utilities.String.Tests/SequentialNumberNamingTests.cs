using Should;
using Xunit;

namespace Utilities.String.Tests
{
    public class SequentialNumberNamingTests
    {
        // TODO: multithreading tests
        [Fact]
        public void SkipsZero()
        {
            var snn = new SequentialNumberNaming();
            const string name = "fnord";
            snn.Get(name).ShouldEqual(name);
            snn.Get(name).ShouldEqual(name + "1");
        }

        [Fact]
        public void AddsZero_If_SkipsZero_IsFalse()
        {
            var snn = new SequentialNumberNaming(false);
            const string name = "fnord";
            snn.Get(name).ShouldEqual(name + "0");
            snn.Get(name).ShouldEqual(name + "1");
        }
    }
}