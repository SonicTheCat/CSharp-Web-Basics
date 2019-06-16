using SIS.MvcFramework.Attributes.Action;
using SULS.Data;
using SULS.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SULS.Services
{
    public class UserService : IUserService
    {
        private readonly SULSContext context;

        public UserService(SULSContext context)
        {
            this.context = context;
        }

        public void CreateUser(string username, string email, string password)
        {
            User user = new User
            {
                Username = username,
                Password = this.HashPassword(password),
                Email = email
            };

            this.context.Users.Add(user);
            this.context.SaveChanges(); 
        }

        public User GetUserByUsernameAndPassword(string username, string password)
        {
            return this.context.Users
                .SingleOrDefault(user => user.Username == username
                  && user.Password == this.HashPassword(password));
        }

        [NonAction]
        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return Encoding.UTF8.GetString(sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password)));
            }
        }
    }
}