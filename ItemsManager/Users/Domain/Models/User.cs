using ItemsManager.Common.Exceptions;
using ItemsManager.Users.Domain.Services;
using System;
using System.Text;

namespace ItemsManager.Users.Domain.Models
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Login { get; private set; }
        public string Firstname { get; private set; }
        public string Email { get; private set; }
        public string Password { get; private set; }
        public byte[] Salt { get; private set; }
        public int Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected User()
        {

        }

        public User(string login, string firstname, string email)
        {
            if (string.IsNullOrWhiteSpace(login))
            {
                throw new SmartFridgeException("empty_user_login", "User login cant be empty.");
            }

            if (string.IsNullOrWhiteSpace(firstname))
            {
                throw new SmartFridgeException("empty_user_name", "User name cant be empty.");
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                //TODO: validate email (with regex?)
                throw new SmartFridgeException("empty_email", "Email cant be empty.");
            }

            Id = Guid.NewGuid();
            Login = login;
            Firstname = firstname;
            Email = email;
            Role = 2;
            CreatedAt = DateTime.UtcNow;
        }

        public void SetPassword(string pswd, IEncrypter encrypter)
        {
            if (string.IsNullOrWhiteSpace(pswd))
            {
                //TODO: Validate password power?
                throw new SmartFridgeException("empty_password", "Password cant be empty.");
            }
            
            Salt = encrypter.CreateSalt(8);
            Password = Encoding.UTF8.GetString(encrypter.GenerateSaltedHash(Encoding.UTF8.GetBytes(pswd), Salt));
        }

        public void SetRole(int role)
        {
            Role = role;
        }

        public bool ValidatePassword(string pswd, IEncrypter encrypter)
        {
            return Password.Equals(encrypter.GenerateSaltedHash(Encoding.UTF8.GetBytes(pswd), Salt));
        }
    }
}
