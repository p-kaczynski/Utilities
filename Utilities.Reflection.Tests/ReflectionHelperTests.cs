using System.Collections.Generic;
using Should;
using Xunit;

namespace Utilities.Reflection.Tests
{
    using static ReflectionHelper;
    public class ReflectionHelperTests
    {
        [Fact]
        public void GetGenericEnumerableItemType_Verify()
        {
            GetGenericEnumerableItemType(typeof(string)).ShouldEqual(typeof(char));
            GetGenericEnumerableItemType(typeof(string[])).ShouldEqual(typeof(string));
            GetGenericEnumerableItemType(typeof(List<int>)).ShouldEqual(typeof(int));
            GetGenericEnumerableItemType(typeof(List<string[]>)).ShouldEqual(typeof(string[]));
            GetGenericEnumerableItemType(typeof(Dictionary<string,List<int>>)).ShouldEqual(typeof(KeyValuePair<string, List<int>>));
            GetGenericEnumerableItemType(typeof(HashSet<double>)).ShouldEqual(typeof(double));



            GetGenericEnumerableItemType(typeof(int)).ShouldBeNull();
        }
    }
}