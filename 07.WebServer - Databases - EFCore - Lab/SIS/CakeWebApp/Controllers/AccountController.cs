namespace CakeWebApp.Controllers
{
    using CakeWebApp.Models;
    using Common;
    using CakeWebApp.Services;
    using CakeWebApp.Services.Contracts;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using System.IO;
    using SIS.HTTP.Enums;

    public class AccountController : BaseController
    {
        private const string LoginViewName = "Login";
        private const string RegisterViewName = "Register";
      
        private readonly IHashService hashService;

        public AccountController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            return this.View(LoginViewName);
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            return this.View(RegisterViewName);
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Db.Users.FirstOrDefault(x =>
             x.Username == username &&
             x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestError("Invalid username or password!");
            }

            var cookieContent = this.UserCookieService.GetUserCookie(user.Username);
            var response = new RedirectResult("/");
            var cookie = new HttpCookie(GlobalConstants.AuthCookieKeyName, cookieContent, 10)
            {
                IsHttpOnly = true
            };
            response.Cookies.Add(cookie);
            return response;
        }

        public IHttpResponse DoRegistration(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            if (string.IsNullOrWhiteSpace(username) || username.Length < 4)
            {
                return this.BadRequestError("Invalid username! It must be at least four symbols long.");
            }

            if (Db.Users.Any(x => x.Username == username))
            {
                return this.BadRequestError($"Username {username} is already registered!");
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return this.BadRequestError($"Weak Password! It must be at least 6 symbols long.");
            }

            if (password != confirmPassword)
            {
                return this.BadRequestError($"Invalid Password! Passwords do not match!");
            }

            var hashedPassword = this.hashService.Hash(password);
            var user = new User
            {
                Name = username,
                Username = username,
                Password = hashedPassword
            };

            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.ServerError(e.Message);
            }

            return new RedirectResult("/");
        }

        public IHttpResponse ShowProfile(IHttpRequest request)
        {
            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);

            if (cookie == null)
            {
                return new RedirectResult("/login");
            }

            var username = this.UserCookieService.GetUserData(cookie.Value);
            var user = this.Db.Users.FirstOrDefault(x => x.Username == username);

            var content = File.ReadAllText("Views/Profile.html") +
                $"<p>Username: {user.Username}</p>" +
                $"<p>DateOfRegistration: {user.DateOfRegistration.ToShortDateString()}</p>" +
                $"<p>Orders Count: {user.Orders.Count}</p>";

            return new HtmlResult(content, SIS.HTTP.Enums.HttpResponseStatusCode.Ok);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            var cookie = request.Cookies.GetCookie(GlobalConstants.AuthCookieKeyName);

            if (cookie == null)
            {
                return new RedirectResult("/");
            }

            cookie.Delete();
            var response = new RedirectResult("/");
            response.Cookies.Add(cookie);

            return response; 
        }
    }
}