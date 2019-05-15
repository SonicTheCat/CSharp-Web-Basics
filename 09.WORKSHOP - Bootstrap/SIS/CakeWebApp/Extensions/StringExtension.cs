namespace CakeWebApp.Extensions
{
    using System.Web;

    public static class StringExtension
    {
        public static string UrlDecode(this string data)
        {
            return HttpUtility.UrlDecode(data);
        }
    }
}