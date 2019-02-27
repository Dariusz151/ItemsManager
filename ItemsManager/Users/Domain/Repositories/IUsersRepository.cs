using ItemsManager.Users.Domain.Models;
using System.Threading.Tasks;

namespace ItemsManager.Users.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetAsync(string login);
        Task<bool> RegisterAsync(User user);
    }
}
