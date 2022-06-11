using AuthProject.Models;

namespace AuthProject.Services
{
    public interface IUserService
    {
         List<User> GetUsers();

         User GetUser(string id);

         User Create(User user);

        public string Authenticate(string email, string password);
    }
}
