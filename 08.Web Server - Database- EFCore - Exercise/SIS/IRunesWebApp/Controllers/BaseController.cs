namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.Common;
    using IRunesWebApp.Data;
    using IRunesWebApp.Services;
    using IRunesWebApp.Services.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.IO;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    public abstract class BaseController
    {
        private const string ViewsFolderName = "Views";

        private const string RootDirectoryPath = "../../../";

        private const string Separator = "/";

        private const string FileExtension = ".html";

        private const string ControllerNameAsString = "Controller";

        public BaseController()
        {
            this.Db = new IRunesDbContext();
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>(); 
        }

        protected IRunesDbContext Db { get; }

        protected IUserCookieService UserCookieService { get; }

        protected IDictionary<string, string> ViewBag { get; set; }

        private string GetCurrentControllerName()
        {
            return this.GetType().Name.Replace(ControllerNameAsString, string.Empty);
        }

        public IHttpResponse View([CallerMemberName] string viewName = "")
        {
            //find the view that has to be rendered
            var path = RootDirectoryPath +
                ViewsFolderName +
                Separator +
                this.GetCurrentControllerName() +
                Separator +
                viewName +
                FileExtension;

            if (!File.Exists(path))
            {
                return new BadRequestResult($"View {viewName} does not exist!", HttpResponseStatusCode.NotFound);
            }

            string content = File.ReadAllText(path);
            
            //insert username in html file
            foreach (var key in this.ViewBag.Keys)
            {
                if (content.Contains($"{{{key}}}"))
                {
                    content = content.Replace($"{{{{{key}}}}}", this.ViewBag[key]); 
                }
            }

            return new HtmlResult(content, HttpResponseStatusCode.Ok);
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            return request.Session.ContainsParameter("username"); 
        }
        
        public void SignInUser(string username, IHttpResponse response, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);
            var cookieContent = this.UserCookieService.GetUserCookie(username);
            var cookie = new HttpCookie(GlobalConstants.AuthCookieKeyName, cookieContent, 10)
            {
                IsHttpOnly = true
            };
            response.Cookies.Add(cookie);
        }
    }
}