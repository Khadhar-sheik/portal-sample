using System.Threading.Tasks;
using Org.Common.Model;

namespace Org.Common.Manager
{
    public interface IUserManager
    {
        Task<User> Register(RegistrationModel model);
        Task<User> Login(string login, string password);
    }
}