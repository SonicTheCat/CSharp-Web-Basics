namespace SIS.WebServer.Results
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Headers;
    using SIS.HTTP.Responses;
    using System;
    using System.Text;

    public class BadRequestResult : HttpResponse
    {
        private const string Error = "<h1>Bad Request!!!</h1>"; 

        public BadRequestResult(string content, HttpResponseStatusCode responseStatusCode)
           : base(responseStatusCode)
        {
            content = Error + Environment.NewLine + content; 
            this.Headers.Add(new HttpHeader("Content-Type", "text/html"));
            this.Content = Encoding.UTF8.GetBytes(content);
        }
    }
}