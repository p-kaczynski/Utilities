using System;
using System.Collections.Generic;
using Should;
using Xunit;

namespace Utilities.DateTime.Tests
{
    public class DateTimeExtensionsTests
    {
        private static readonly Random _random = new Random();
        private static long LongRandom(long min, long max)
        {
            var buf = new byte[8];
            _random.NextBytes(buf);
            var longRand = BitConverter.ToInt64(buf, 0);

            return Math.Abs(longRand % (max - min)) + min;
        }
        private static IEnumerable<object[]> GetDates()
        {
            for (var i = 0; i < 5; ++i)
            {
                var before = new System.DateTime(LongRandom(System.DateTime.MinValue.Ticks,System.DateTime.MaxValue.Ticks));
                var after = before + new TimeSpan(LongRandom(System.DateTime.MinValue.Ticks, System.DateTime.MaxValue.Ticks - before.Ticks));
                // get middle:
                var between = new System.DateTime((before.Ticks + after.Ticks) / 2);

                yield return new object[]{between, before, after};
            }
        }

        [Theory]
        [MemberData(nameof(GetDates))]
        public void BetweenAutoTest(System.DateTime thisDate, System.DateTime before, System.DateTime after)
        {
            thisDate.Between(before,after).ShouldBeTrue();
            after.Between(before, thisDate).ShouldBeFalse();
        }
    }
}
