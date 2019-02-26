using ItemsManager.Users.Commands;
using ItemsManager.Users.Domain.Models;
using System;
using System.Threading.Tasks;

namespace ItemsManager.Users.Domain.Repositories
{
    public interface IUsersRepository
    {
        Task<User> GetAsync(string login);

        Task<Guid> LoginAsync(LoginUser user);
        Task<bool> RegisterAsync(User user);
    }
}
