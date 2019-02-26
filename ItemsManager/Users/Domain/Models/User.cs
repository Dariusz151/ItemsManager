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
        public string Salt { get; private set; }
        public int Role { get; private set; }
        public DateTime CreatedAt { get; private set; }

        protected User()
        {

        }

        public User(Guid id, string password, string salt)
        {
            Id = id;
            Password = password;
            Salt = salt;
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
        
        public void SetPassword(string password, IEncrypter encrypter)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                //TODO: Validate password power?
                throw new SmartFridgeException("empty_password", "Password cant be empty.");
            }
            
            Salt = encrypter.CreateSalt(8);
            Password = encrypter.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(Salt));
        }

        public void SetRole(int role)
        {
            Role = role;
        }

        public bool ValidatePassword(string password, string salt, IEncrypter encrypter)
        {
            var hashedPassword = encrypter.GenerateSaltedHash(Encoding.UTF8.GetBytes(password), Encoding.UTF8.GetBytes(salt));
            
            return Password.Equals(hashedPassword);
        }
    }
}
