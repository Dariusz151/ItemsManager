using ItemsManager.Common.Auth;
using ItemsManager.Common.Exceptions;
using ItemsManager.Users.Domain.Models;
using ItemsManager.Users.Domain.Repositories;
using ItemsManager.Users.Domain.Services;
using System.Threading.Tasks;

namespace ItemsManager.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IEncrypter _encrypter;
        private readonly IJwtHandler _jwtHandler;

        public UsersService(IUsersRepository repository,
            IEncrypter encrypter,
            IJwtHandler jwtHandler)
        {
            _repository = repository;
            _encrypter = encrypter;
            _jwtHandler = jwtHandler;
        }

        public async Task<JsonWebToken> LoginAsync(string login, string password)
        {
            var user = await _repository.GetAsync(login);

            if (user is null)
            {
                throw new SmartFridgeException("invalid_login", "Invalid login");
            }

            if (!user.ValidatePassword(password, user.Salt, _encrypter))
            {
                throw new SmartFridgeException("invalid_password", "Invalid password");
            }
            return _jwtHandler.Create(user.Id);
        }

        public async Task<bool> RegisterAsync(string login, string email, string password, string firstname)
        {
            bool isSuccess = false;
            var user = await _repository.GetAsync(login);
            if (user != null)
            {
                throw new SmartFridgeException("invalid_login", "Login is already in use.");
            }

            user = new User(login, firstname, email);
            user.SetPassword(password, _encrypter);

            isSuccess = await _repository.RegisterAsync(user);

            return isSuccess;
        }
    }
}
