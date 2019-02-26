﻿using System.Threading.Tasks;

namespace ItemsManager.Users.Services
{
    public interface IUsersService
    {
        Task<bool> RegisterAsync(string login, string email, string password, string firstname);
        Task<bool> LoginAsync(string email, string password);
    }
}
