﻿using ItemsManager.Common.Auth;
using System.Threading.Tasks;

namespace ItemsManager.Users.Services
{
    public interface IUsersService
    {
        Task<bool> RegisterAsync(string login, string email, string password, string firstname);
        Task<JsonWebToken> LoginAsync(string login, string password);
    }
}
