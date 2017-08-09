using System;
using Should;
using Utilities.Collections.Arrays;
using Xunit;

namespace Utilities.Collections.Tests.Arrays
{
    public class ArrayExtensionsTests
    {
        [Fact]
        public void Splice_BasicTest()
        {
            var testArray = new [] {0, 1, 2, 3, 4, 5, 6};
            var resultArray = testArray.Splice(2, 3);
            resultArray.ShouldNotBeNull();
            resultArray.Length.ShouldEqual(2);
            resultArray[0].ShouldEqual(2);
            resultArray[1].ShouldEqual(3);
        }

        [Fact]
        public void Splice_EmptyArray_ReturnsEmpty()
        {
            var result = new int[0].Splice(3, 3);
            result.ShouldNotBeNull();
            result.Length.ShouldEqual(0);
        }

        [Fact]
        public void Splice_OverlappingMargins_ReturnsEmpty()
        {
            var result = new[] {1, 2, 3, 4, 5, 6}.Splice(4, 3);
            result.ShouldNotBeNull();
            result.Length.ShouldEqual(0);
        }

        [Fact]
        public void Splice_Null_ThrowsException()
        {
            int[] array = null;
            new Action(()=>array.Splice(1,2)).ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void Splice_NegativeMargins_ThrowsException()
        {
            var array = new[] {1, 2, 3, 4};
            new Action(()=>array.Splice(-1,2)).ShouldThrow<ArgumentException>();
            new Action(()=>array.Splice(1,-2)).ShouldThrow<ArgumentException>();
            new Action(()=>array.Splice(-1,-2)).ShouldThrow<ArgumentException>();
        }

        //[Fact]
        //public void Append_Adds_elements()
        //{
        //    var array = new[] {1, 2, 3, 4};
        //    array.Append(5, 6);
        //    array.Length.ShouldEqual(6);
        //    array[4].ShouldEqual(5);
        //    array[5].ShouldEqual(6);
        //}
    }
}