using ItemsManager.Common.Exceptions;
using ItemsManager.Users.Domain;
using ItemsManager.Users.Domain.Models;
using ItemsManager.Users.Domain.Repositories;
using ItemsManager.Users.Domain.Services;
using ItemsManager.Users.Repositories;
using System;
using System.Threading.Tasks;

namespace ItemsManager.Users.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IEncrypter _encrypter;

        public UsersService(IUsersRepository repository,
            IEncrypter encrypter)
        {
            _repository = repository;
            _encrypter = encrypter;
        }

        public Task<bool> LoginAsync(string email, string password)
        {
            throw new NotImplementedException();
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
