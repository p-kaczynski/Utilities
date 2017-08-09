using System.Collections.Generic;
using System.Linq;
using Should;
using Xunit;

namespace Utilities.Collections.Tests
{
    public class ReadOnlyExtensionsTests
    {
        [Fact]
        public void ToReadOnlyCollection_FromEnumerable()
        {
            ((IEnumerable<int>) null).ToReadOnlyCollection().ShouldBeNull();
            var notNull = Enumerable.Range(1, 5).ToReadOnlyCollection();
            notNull.ShouldNotBeNull();
        }

        [Fact]
        public void ToReadOnlyCollection_FromList()
        {
            ((List<int>)null).ToReadOnlyCollection().ShouldBeNull();
            var notNull = new List<int>{1,2,3,4,5}.ToReadOnlyCollection();
            notNull.ShouldNotBeNull();
        }

        [Fact]
        public void ToReadOnlyCollection_FromArray()
        {
            ((int[])null).ToReadOnlyCollection().ShouldBeNull();
            var notNull = new [] { 1, 2, 3, 4, 5 }.ToReadOnlyCollection();
            notNull.ShouldNotBeNull();
        }

        [Fact]
        public void ToReadOnlyList_FromEnumerable()
        {
            ((IEnumerable<int>)null).ToReadOnlyList().ShouldBeNull();
            var notNull = Enumerable.Range(1, 5).ToReadOnlyList();
            notNull.ShouldNotBeNull();
        }

        [Fact]
        public void ToReadOnlyList_FromList()
        {
            ((List<int>)null).ToReadOnlyList().ShouldBeNull();
            var notNull = new List<int> { 1, 2, 3, 4, 5 }.ToReadOnlyList();
            notNull.ShouldNotBeNull();
        }

        [Fact]
        public void ToReadOnlyList_FromArray()
        {
            ((int[])null).ToReadOnlyList().ShouldBeNull();
            var notNull = new[] { 1, 2, 3, 4, 5 }.ToReadOnlyList();
            notNull.ShouldNotBeNull();
        }
    }
}