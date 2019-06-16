using SIS.MvcFramework;
using SIS.MvcFramework.Attributes;
using SIS.MvcFramework.Attributes.Security;
using SIS.MvcFramework.Result;
using SULS.App.ViewModels.Users;
using SULS.Models;
using SULS.Services;

namespace SULS.App.Controllers
{
    public class UsersController : Controller
    {
        private const string UsersRegisterPage = "/Users/Register";
        private const string UsersLoginPage = "/Users/Login";
        private const string HomePage = "/";

        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Login()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Login(UserLoginInputModel model)
        {
            User userFromDb = this.userService.GetUserByUsernameAndPassword(model.Username, model.Password);

            if (userFromDb == null)
            {
                return this.Redirect(UsersLoginPage);
            }

            this.SignIn(userFromDb.Id, userFromDb.Username, userFromDb.Email);

            return this.Redirect(HomePage);
        }

        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterInputModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.Redirect(UsersRegisterPage);
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Redirect(UsersRegisterPage);
            }

            this.userService.CreateUser(model.Username, model.Email, model.Password);

            return this.Redirect(UsersLoginPage);
        }

        [Authorize]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect(HomePage);
        }
    }
}