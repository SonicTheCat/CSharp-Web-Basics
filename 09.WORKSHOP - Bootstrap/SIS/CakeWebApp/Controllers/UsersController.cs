namespace CakeWebApp.Controllers
{
    using CakeWebApp.Extensions;
    using CakeWebApp.Models;
    using CakeWebApp.Services;
    using CakeWebApp.Services.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController()
        {
            this.hashService = new HashService();
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return new RedirectResult("/");
            }

            return this.View();
        }

        public IHttpResponse Register(IHttpRequest request)
        {
            if (this.IsAuthenticated(request))
            {
                return new RedirectResult("/");
            }

            return this.View();
        }

        public IHttpResponse DoLogin(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim().UrlDecode();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);
            var user = this.Db.Users
                .FirstOrDefault(x => x.Username == username && x.Password == hashedPassword);

            if (user == null)
            {
                return new BadRequestResult("Wrong username or password!", HttpResponseStatusCode.BadRequest);
            }

            return PackUpResponse(username, request);
        }

        public IHttpResponse DoRegister(IHttpRequest request)
        {
            var fullname = request.FormData["fullname"].ToString().Trim().UrlDecode();
            var username = request.FormData["username"].ToString().Trim().UrlDecode();
            var password = request.FormData["password"].ToString();
            var confirmPassword = request.FormData["confirmPassword"].ToString();

            //TODO: Validate data!

            var hashedPassword = this.hashService.Hash(password);
            var user = new User()
            {
                Name = fullname,
                Username = username,
                Password = hashedPassword,
            };

            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return new BadRequestResult(e.Message, HttpResponseStatusCode.BadRequest);
            }

            return PackUpResponse(user.Username, request);
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            var response = new RedirectResult("/users/login");
            this.SignOutUser(request, response);

            return response;
        }

        public IHttpResponse Profile(IHttpRequest request)
        {
            if (!this.IsAuthenticated(request))
            {
                return new RedirectResult("/users/login");
            }

            var username = request.Session.GetParameter("username").ToString();
            var user = this.Db.Users.FirstOrDefault(x => x.Username == username);

            this.ViewBag["fullname"] = user.Name; 
            this.ViewBag["username"] = user.Username; 
            this.ViewBag["registered"] = user.DateOfRegistration.ToShortDateString(); 
            this.ViewBag["orders"] = user.Orders.Count.ToString();

            return this.View(); 
        }

        private IHttpResponse PackUpResponse(string username, IHttpRequest request)
        {
            var response = new RedirectResult("/");
            this.SignInUser(username, response, request);

            return response;
        }
    }
}