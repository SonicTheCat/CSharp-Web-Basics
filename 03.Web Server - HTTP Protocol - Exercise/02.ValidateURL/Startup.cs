namespace _02.ValidateURL
{
    using System;
    using System.Net;

    public class Startup
    {
        public static void Main()
        {
            var url = WebUtility.UrlDecode(Console.ReadLine());
            var result = ValidateUrl(url);
            Console.WriteLine(result);
        }

        private static string ValidateUrl(string url)
        {
            bool isValid = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult);
            var result = "Invalid URL";

            if (!isValid)
            {
                return result;
            }

            if (uriResult.Scheme != "https" &&
                uriResult.Scheme != "http" &&
                uriResult.Scheme != "ftp")
            {
                return result;
            }

            if ((uriResult.Scheme == "https" && uriResult.Port == 80) ||
                (uriResult.Scheme == "http" && uriResult.Port == 443))
            {
                return result;
            }

            return GetUrlParts(uriResult);
        }

        private static string GetUrlParts(Uri uriResult)
        {
            var result = "Protocol: " + uriResult.Scheme + Environment.NewLine
                + "Host: " + uriResult.Host + Environment.NewLine
                + "Port: " + uriResult.Port + Environment.NewLine
                + "Path: " + uriResult.AbsolutePath;

            if (!string.IsNullOrEmpty(uriResult.Query))
            {
                result += Environment.NewLine + "Query: " + uriResult.Query;
            }

            if (!string.IsNullOrEmpty(uriResult.Fragment))
            {
                result += Environment.NewLine + "Fragment: " + uriResult.Fragment;
            }

            return result;
        }
    }
}