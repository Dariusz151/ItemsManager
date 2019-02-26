using ItemsManager.Users.Commands;
using ItemsManager.Users.Domain;
using System;
using System.Threading.Tasks;

namespace ItemsManager.Users.Repositories
{
    public interface IUsersRepository
    {
        Task<Guid> LoginAsync(LoginUser user);
        Task<bool> RegisterAsync(User user);
    }
}
