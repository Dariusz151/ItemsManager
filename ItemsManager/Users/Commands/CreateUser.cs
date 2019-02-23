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
        public string Phone { get; }
        public string Password { get; }
        public int Role { get; }

        [JsonConstructor]
        public CreateUser(string login, string fname, string email, string phone, string pswd, int role)
        {
            Login = login;
            Firstname = fname;
            Email = email;
            Phone = phone;
            Password = pswd;
            Role = role;
        }
    }
}
