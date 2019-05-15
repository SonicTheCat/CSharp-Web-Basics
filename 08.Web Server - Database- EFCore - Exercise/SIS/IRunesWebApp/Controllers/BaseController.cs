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

        protected bool IsLoggedIn { get; set; }

        public IHttpResponse View([CallerMemberName] string viewName = "")
        {
            string path = GetCurrentViewPath(viewName);

            if (!File.Exists(path))
            {
                return new BadRequestResult($"View {viewName} does not exist!", HttpResponseStatusCode.NotFound);
            }

            string content = ReplacePlaceholdersInView(path);
            this.ViewBag["renderBody"] = content;
            var layoutContent = ReplacePlaceholdersInView(GetLayoutPath());

            return new HtmlResult(layoutContent, HttpResponseStatusCode.Ok);
        }

        public bool IsAuthenticated(IHttpRequest request)
        {
            if (request.Session.ContainsParameter("username"))
            {
                this.ViewBag["loggedOut"] = "none";
                this.ViewBag["loggedIn"] = "inline";
                this.IsLoggedIn = true;
            }
            else
            {
                this.ViewBag["loggedOut"] = "inline";
                this.ViewBag["loggedIn"] = "none";
                this.IsLoggedIn = false;
            }

            return this.IsLoggedIn;
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

        public void SignOutUser(IHttpRequest request, IHttpResponse response)
        {
            request.Session.ClearParameters();
            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);

            if (cookie != null)
            {
                cookie.Delete();
                response.AddCookie(cookie);
            }
        }

        private string GetCurrentControllerName()
        {
            return this.GetType().Name.Replace(ControllerNameAsString, string.Empty);
        }

        private string GetCurrentViewPath(string viewName)
        {
            //find the view that has to be rendered
            return RootDirectoryPath +
                ViewsFolderName +
                Separator +
                this.GetCurrentControllerName() +
                Separator +
                viewName +
                FileExtension;
        }

        private static string GetLayoutPath()
        {
            return RootDirectoryPath +
                ViewsFolderName +
                Separator +
                "_Layout" +
                FileExtension;
        }

        private string ReplacePlaceholdersInView(string path)
        {
            string content = File.ReadAllText(path);

            foreach (var key in this.ViewBag.Keys)
            {
                if (content.Contains($"{{{key}}}"))
                {
                    content = content.Replace($"{{{{{key}}}}}", this.ViewBag[key]);
                }
            }
            return content;
        }
    }
}