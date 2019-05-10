namespace IRunesWebApp.Controllers
{
    using IRunesWebApp.Common.Validators;
    using IRunesWebApp.Models;
    using IRunesWebApp.Services;
    using IRunesWebApp.Services.Contracts;
    using SIS.HTTP.Enums;
    using SIS.HTTP.Requests.Contracts;
    using SIS.HTTP.Responses.Contracts;
    using SIS.WebServer.Results;
    using System;
    using System.Linq;
    using System.Web;

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

        public IHttpResponse LoginPostRequest(IHttpRequest request)
        {
            var usernameOrMail = request.FormData["usernameOrEmail"].ToString().Trim();
            var password = request.FormData["password"].ToString();

            var hashedPassword = this.hashService.Hash(password);

            var user = this.Db.Users.FirstOrDefault(x =>
                (x.Username == usernameOrMail || x.Email == usernameOrMail)
                && x.HashedPassword == hashedPassword);

            if (user == null)
            {
                return new BadRequestResult("Invalid username or Password!", HttpResponseStatusCode.BadRequest);
            }

            return PackUpResponse(user.Username, request);
        }

        public IHttpResponse RegisterPostRequest(IHttpRequest request)
        {
            var username = request.FormData["username"].ToString().Trim();
            var password = request.FormData["password"].ToString();
            var confirmedPassword = request.FormData["confirmPassword"].ToString();
            var email = HttpUtility.UrlDecode(request.FormData["email"].ToString());

            if (!UserValidator.IsUsernameValid(username, this.Db))
            {
                return new BadRequestResult("Invalid username!", HttpResponseStatusCode.BadRequest);
            }
            else if (!UserValidator.IsEmailValid(email, this.Db))
            {
                return new BadRequestResult("Invalid email address!", HttpResponseStatusCode.BadRequest);
            }
            else if (!UserValidator.IsPasswordValid(password))
            {
                return new BadRequestResult("Invalid password!", HttpResponseStatusCode.BadRequest);
            }
            else if (!UserValidator.IsConfirmPasswordValid(password, confirmedPassword))
            {
                return new BadRequestResult("Password and confirm password do not match!", HttpResponseStatusCode.BadRequest);
            }

            var hashedPassword = this.hashService.Hash(password);
            var user = new User()
            {
                Username = username,
                HashedPassword = hashedPassword,
                Email = email
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

        private IHttpResponse PackUpResponse(string username, IHttpRequest request)
        {
            var response = new RedirectResult("/");
            this.SignInUser(username, response, request);

            return response;
        }
    }
}