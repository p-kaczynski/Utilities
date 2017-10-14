using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Should;
using Utilities.Collections.Enumerables;
using Xunit;

namespace Utilities.Collections.Tests.Enumerables
{
    public class EnumerableExtensionsTests
    {
        public static readonly Random Random = new Random();

        [Theory]
        [MemberData(nameof(IntertwineDataProvider), 10, true)]
        public void Intertwine_Even(IEnumerable<int> source, IEnumerable<int> other)
        {
            var sourceArr = source.ToArray();
            var otherArr = other.ToArray();

            var result = sourceArr.Intertwine(otherArr).ToArray();

            result.ShouldNotBeNull();
            result.Length.ShouldEqual(sourceArr.Length + otherArr.Length);
            for (var i = 0; i < sourceArr.Length; ++i)
            {
                result[i * 2].ShouldEqual(sourceArr[i]);
            }
            for (var i = 0; i < otherArr.Length; ++i)
            {
                result[i * 2 + 1].ShouldEqual(otherArr[i]);
            }
        }

        [Theory]
        [MemberData(nameof(IntertwineDataProvider), 10, false)]
        public void Intertwine_Uneven(IEnumerable<int> source, IEnumerable<int> other)
        {
            var sourceArr = source.ToArray();
            var otherArr = other.ToArray();

            var result = sourceArr.Intertwine(otherArr).ToArray();

            result.ShouldNotBeNull();
            result.Length.ShouldEqual(sourceArr.Length + otherArr.Length);
            var shorterLength = Math.Min(sourceArr.Length, otherArr.Length);
            for (var i = 0; i < sourceArr.Length; ++i)
            {
                result[i > shorterLength - 1 ? shorterLength + i : 2*i ].ShouldEqual(sourceArr[i]);
            }
            for (var i = 0; i < otherArr.Length; ++i)
            {
                result[i > shorterLength -1 ? shorterLength + i : 2 * i + 1].ShouldEqual(otherArr[i]);
            }
        }

        public static IEnumerable<object[]> IntertwineDataProvider(int howMany, bool equal)
        {
            return Enumerable.Range(0, howMany).Select(_ =>
            {
                var firstLength = Random.Next(1, 1000);
                var secondLength = equal ? firstLength : Random.Next(1, 1000);

                return new object[]{Enumerable.Range(0,firstLength).Select(__=>Random.Next()), Enumerable.Range(0,secondLength).Select(__=>Random.Next())};
            });
        }
    }
}