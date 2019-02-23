﻿using ItemsManager.Common.Types;
using Newtonsoft.Json;

namespace ItemsManager.Users.Commands
{
    //immutable
    public class CreateUser : ICommand
    {
        public string Login { get; }
        public string Firstname { get; }
        public string Email { get; }
        public string Phone { get; }
        public string Password { get; }
        public int Role { get; }

        [JsonConstructor]
        public CreateUser(string login, string firstname, string email, string phone, string password, int role)
        {
            Login = login;
            Firstname = firstname;
            Email = email;
            Phone = phone;
            Password = password;
            Role = role;
        }
    }
}
