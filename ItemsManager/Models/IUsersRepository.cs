using System.Threading.Tasks;

namespace SmartFridge.Models
{
    public interface IUsersRepository
    {
        Task<int> LoginAsync(UserDTO user);
        Task<int> RegisterAsync(UserDTO user);
    }
}
