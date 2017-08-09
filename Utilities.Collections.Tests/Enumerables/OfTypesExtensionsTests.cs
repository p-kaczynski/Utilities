using System;
using System.Collections.Generic;
using System.Linq;
using Should;
using Utilities.Collections.Enumerables;
using Xunit;

namespace Utilities.Collections.Tests.Enumerables
{
    public class OfTypesExtensionsTests
    {
        private class UnderTest{ }
        private class UnderTestSub1 : UnderTest { }
        private class UnderTestSub2 : UnderTest { }
        private class UnderTestSub3 : UnderTest { }
        private class UnderTestSub4 : UnderTest { }
        private class UnderTestSub5 : UnderTest { }
        private class UnderTestSub6 : UnderTest { }

        [Fact]
        public void OfTypes_FiltersCorrectly()
        {
            var collection = new UnderTest[]
            {
                // 2 of each aside of sub6
                new UnderTestSub1(),
                new UnderTestSub2(),
                new UnderTestSub3(),
                new UnderTestSub4(),
                new UnderTestSub5(),
                new UnderTestSub1(),
                new UnderTestSub2(),
                new UnderTestSub3(),
                new UnderTestSub4(),
                new UnderTestSub5(),
            };

            // 2 types
            collection.OfTypes(typeof (UnderTestSub1), typeof (UnderTestSub2)).Count().ShouldEqual(2 * 2);

            // 2 types, one missing
            collection.OfTypes(typeof(UnderTestSub1), typeof(UnderTestSub6)).Count().ShouldEqual(2);
        }

        [Fact]
        public void OfTypes_ThrowsForNullCollection()
        {
            // ReSharper disable once AssignNullToNotNullAttribute - intentional
            new Action(()=> ((IEnumerable<UnderTest>)null).OfTypes()).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void OfTypes_IgnoresNullsInCollection()
        {
            var collection = new UnderTest[]
            {
                // 2 of each aside of sub6
                new UnderTestSub1(),
                new UnderTestSub2(),
                new UnderTestSub3(),
                new UnderTestSub4(),
                new UnderTestSub5(),
                null,
                new UnderTestSub1(),
                new UnderTestSub2(),
                new UnderTestSub3(),
                null,
                new UnderTestSub4(),
                new UnderTestSub5(),
                null,
                null
            };

            // 2 types
            collection.OfTypes(typeof(UnderTestSub1), typeof(UnderTestSub2)).Count().ShouldEqual(2 * 2);

            // 2 types, one missing
            collection.OfTypes(typeof(UnderTestSub1), typeof(UnderTestSub6)).Count().ShouldEqual(2);
        }
    }
}