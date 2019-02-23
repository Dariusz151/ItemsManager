using ItemsManager.Common.Types;

namespace ItemsManager.Users.Commands
{
    public class LoginUser : ICommand
    {
        public string Login { get; private set; }
        public string Password { get; private set; }

        public LoginUser(string login, string pswd)
        {
            Login = login;
            Password = pswd;
        }
    }
}
