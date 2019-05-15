namespace CakeWebApp.Controllers
{
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System.IO;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using CakeWebApp.Data;
    using CakeWebApp.Services;
    using CakeWebApp.Common;

    public abstract class BaseController
    {
        private const string ViewsFolderName = "Views";

        private const string RootDirectoryPath = "../../../";

        private const string Separator = "/";

        private const string FileExtension = ".html";

        private const string ControllerNameAsString = "Controller";
        
        private const string Layouts = "Layouts";

        public BaseController()
        {
            this.Db = new CakeDbContext();
            this.UserCookieService = new UserCookieService();
            this.ViewBag = new Dictionary<string, string>();
        }

        protected CakeDbContext Db { get; }

        protected UserCookieService UserCookieService { get; }

        protected IDictionary<string, string> ViewBag { get; set; }

        protected bool IsLoggedIn { get; set; }

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

            var layout = "_Layout-LoggedIn"; 
            if (!this.IsLoggedIn)
            {
                layout = "_Layout-LoggedOut";
            }

            var mainPath = RootDirectoryPath + ViewsFolderName + Separator + Layouts + Separator + layout + FileExtension;

            var mainContent = File.ReadAllText(mainPath);

            mainContent = mainContent.Replace("{{mainContent}}", content);

            return new HtmlResult(mainContent, HttpResponseStatusCode.Ok);
        }

        protected bool IsAuthenticated(IHttpRequest request)
        {
            if (request.Session.ContainsParameter("username"))
            {
                this.IsLoggedIn = true;
            }
            else
            {
                this.IsLoggedIn = false;
            }

            return this.IsLoggedIn;
        }

        protected void SignInUser(string username, IHttpResponse response, IHttpRequest request)
        {
            request.Session.AddParameter("username", username);
            var cookieContent = this.UserCookieService.GetUserCookie(username);
            var cookie = new HttpCookie(GlobalConstants.AuthCookieKeyName, cookieContent, 10)
            {
                IsHttpOnly = true
            };
            response.Cookies.Add(cookie);
        }

        protected void SignOutUser(IHttpRequest request, IHttpResponse response)
        {
            request.Session.ClearParameters();
            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);

            if (cookie != null)
            {
                cookie.Delete();
                response.AddCookie(cookie);
            }
        }
    }
}