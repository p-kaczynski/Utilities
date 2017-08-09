using JetBrains.Annotations;

namespace Utilities.DateTime
{
    public static class DateTimeExtensions
    {
        [PublicAPI]
        public static bool Between(this System.DateTime dateTime, System.DateTime from, System.DateTime to)
        {
            return from < dateTime && dateTime < to;
        }
    }
}