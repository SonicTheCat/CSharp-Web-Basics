namespace _03.RequestParser
{
    using System;
    using System.Collections.Generic;

    public class Startup
    {
        public static void Main()
        {
            var methodPaths = new Dictionary<string, List<string>>();

            string input;
            while ((input = Console.ReadLine()) != "END")
            {
                var tokens = input.Split("/", StringSplitOptions.RemoveEmptyEntries);

                var path = tokens[0].Trim();
                var method = tokens[1].Trim().ToLower();

                if (!methodPaths.ContainsKey(method))
                {
                    methodPaths.Add(method, new List<string>());
                }

                methodPaths[method].Add(path);
            }

            var result = HttpRequest(methodPaths);
            Console.WriteLine(result);
        }

        private static string HttpRequest(Dictionary<string, List<string>> methodPaths)
        {
            var tokens = Console.ReadLine().Split(" ", StringSplitOptions.RemoveEmptyEntries);
            var method = tokens[0].Trim().ToLower();
            var path = tokens[1].TrimStart('/');
            var protocol = tokens[2];
            var status = "200 OK";
            var responseText = "OK";

            if (!methodPaths.ContainsKey(method) ||
                (methodPaths.ContainsKey(method) &&
                !methodPaths[method].Contains(path)))
            {
                status = "404 Not Found";
                responseText = "NotFound";
            }

            return HttpResponse(status, responseText);
        }

        private static string HttpResponse(string status, string responseText)
        {
            var template = "HTTP/1.1 {0}" + Environment.NewLine
              + "Content-Length: {1}" + Environment.NewLine
              + "Content-Type: text/plain" + Environment.NewLine + Environment.NewLine
              + "{2}";

            return string.Format(template, status, responseText.Length, responseText);
        }
    }
}