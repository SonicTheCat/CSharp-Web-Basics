using SULS.Models;

namespace SULS.Services
{
    public interface IUserService
    {
        void CreateUser(string username, string email, string password);

        User GetUserByUsernameAndPassword(string username, string password); 
    }
}