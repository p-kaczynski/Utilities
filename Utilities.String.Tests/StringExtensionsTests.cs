using Should;
using Xunit;

namespace Utilities.String.Tests
{
    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("this:is", ':', "this")]
        [InlineData("fnord", 'o', "fn")]
        [InlineData("I♥NY", '♥', "I")]
        public void Until_Char_Single(string str, char search, string result)
        {
            str.Until(search).ShouldEqual(result);
        }

        [Theory]
        [InlineData("this:is:sparta", ':', "this")]
        [InlineData("fnord is dope", 'o', "fn")]
        [InlineData("I♥NY♥♥Hello", '♥', "I")]
        public void Until_Char_Multiple(string str, char search, string result)
        {
            str.Until(search).ShouldEqual(result);
        }

        [Theory]
        [InlineData("this is a string", 'á')]
        [InlineData("12345678", '♥')]
        [InlineData("", '♥')]
        public void Until_Char_NoCharacter(string str, char search)
        {
            str.Until(search).ShouldEqual(str);
        }

        [Theory]
        [InlineData("that is very interesting", "is", "that ")]
        [InlineData("236663423", "666", "23")]
        [InlineData("23666342366664423423", "6666", "236663423")]
        [InlineData("I♥NY", "♥", "I")]
        public void Until_String_Single(string str, string search, string result)
        {
            str.Until(search).ShouldEqual(result);
        }

        [Theory]
        [InlineData("one o clock, two o clock, three o clock", "clock", "one o ")]
        [InlineData("23666342366666532", "666", "23")]
        [InlineData("I♥NY♥♥LA♥♥♥SF", "♥", "I")]
        public void Until_String_Multiple(string str, string search, string result)
        {
            str.Until(search).ShouldEqual(result);
        }

        [Theory]
        [InlineData("this is a string", "fnord")]
        [InlineData("12345678", "I like pancakes")]
        [InlineData("", "a")]
        public void Until_String_NoCharacter(string str, string search)
        {
            str.Until(search).ShouldEqual(str);
        }
    }
}
