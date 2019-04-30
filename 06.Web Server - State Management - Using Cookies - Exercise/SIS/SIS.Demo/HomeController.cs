namespace SIS.Demo
{
    using SIS.HTTP.Enums;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            string content = "<h1>Hello, From my fockin server!</h1>";
            return new HtmlResult(content, HttpResponseStatusCode.Ok); 
        }
    }
}