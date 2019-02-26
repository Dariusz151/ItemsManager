using ItemsManager.Common.Types;
using Newtonsoft.Json;

namespace ItemsManager.Users.Commands
{
    //immutable
    public class CreateUser : ICommand
    {
        public string Login { get; }
        public string Firstname { get; }
        public string Email { get; }
        public string Password { get; }

        [JsonConstructor]
        public CreateUser(string login, string firstname, string email, string password)
        {
            Login = login;
            Firstname = firstname;
            Email = email;
            Password = password;
        }
    }
}
