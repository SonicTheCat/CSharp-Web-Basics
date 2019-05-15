namespace IRunesWebApp.Common.Validators
{
    using IRunesWebApp.Data;
    using SIS.HTTP.Enums;
    using SIS.WebServer.Results;
    using System.Linq;
    using System.Text.RegularExpressions;

    public class UserValidator
    {
        public static bool IsUsernameValid(string username, IRunesDbContext db)
        {
            if (string.IsNullOrEmpty(username) || db.Users.Any(x => x.Username == username))
            {
                return false;
            }

            return true;
        }

        public static bool IsEmailValid(string email, IRunesDbContext db)
        {
            if (string.IsNullOrEmpty(email) || db.Users.Any(x => x.Email == email))
            {
                return false; 
            }

            //Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            //Match match = regex.Match(email);
            //if (!match.Success)
            //{
            //    return false; 
            //}

            return true; 
        }

        public static bool IsPasswordValid(string password)
        {
            if (string.IsNullOrEmpty(password) ||
              string.IsNullOrWhiteSpace(password) ||
              password.Length < 6 )
            {
                return false;
            }

            return true;
        }

        public static bool IsConfirmPasswordValid(string password, string confirmPassword)
        {
            if (password != confirmPassword)
            {
                return false;
            }

            return true;
        }
    }
}