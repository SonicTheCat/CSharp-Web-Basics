namespace CakeWebApp.Controllers
{
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;

    public class HomeController : BaseController
    {
        private const string ViewName = "Home";

        public IHttpResponse Index(IHttpRequest request)
        {
            return this.View(ViewName); 
        }

        //public IHttpResponse Hello(IHttpRequest request)
        //{
        //    return new HtmlResult($"<h1>Hello, {this.GetUsername(request)}</h1>", SIS.HTTP.Enums.HttpResponseStatusCode.Ok); 
        //}
    }
}