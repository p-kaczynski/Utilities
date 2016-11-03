namespace Utilities.DateTime
{
    public static class DateTimeExtensions
    {
        public static bool Between(this System.DateTime dateTime, System.DateTime from, System.DateTime to)
        {
            return from < dateTime && dateTime < to;
        }
    }
}