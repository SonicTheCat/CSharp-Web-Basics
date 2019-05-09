namespace CakeWebApp.Controllers
{
    using System.IO;
    using CakeWebApp.Common;
    using CakeWebApp.Data;
    using CakeWebApp.Services;
    using CakeWebApp.Services.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    
    public abstract class BaseController
    {
        private const string FolderName = "Views/";
        private const string FileExtensions = ".html";

        public BaseController()
        {
            this.Db = new CakeDbContext();
            this.UserCookieService = new UserCookieService();
        }
        
        protected CakeDbContext Db { get; }

        protected IUserCookieService UserCookieService { get; }
        
        protected string GetUsername(IHttpRequest request)
        {
            if (!request.Cookies.ContainsCookie(GlobalConstants.AuthCookieKeyName))
            {
                return null; 
            }

            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);
            var cookieContent = cookie.Value;
            var username = this.UserCookieService.GetUserData(cookieContent);
            return username;
        }

        protected IHttpResponse View(string viewName)
        {
            string content = File.ReadAllText(FolderName + viewName + FileExtensions);
            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        protected IHttpResponse BadRequestError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.BadRequest); 
        }

        protected IHttpResponse ServerError(string errorMessage)
        {
            return new HtmlResult($"<h1>{errorMessage}</h1>", HttpResponseStatusCode.InternalServerError);
        }
    }
}