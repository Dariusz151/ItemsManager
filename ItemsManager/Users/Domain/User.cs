using System;

namespace ItemsManager.Users.Domain
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string Firstname { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public string Password { get; private set; }
        public int Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public User(string login, string fname, string email, string phone, string pswd, int role)
        {
            Id = Guid.NewGuid();
            Login = login;
            Firstname = fname;
            Email = email;
            Phone = phone;
            Password = pswd;
            Role = role;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
