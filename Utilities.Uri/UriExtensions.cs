namespace Utilities.Uri
{
    public static class UriExtensions
    {
        public static string WithoutQueryString(this System.Uri uri)
        {
            return $"{uri.Scheme}{System.Uri.SchemeDelimiter}{uri.Authority}{uri.AbsolutePath}";
        }
    }
}