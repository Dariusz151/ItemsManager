using ItemsManager.Common.Exceptions;
using ItemsManager.Users.Domain.Models;
using ItemsManager.Users.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ItemsManager.Users.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private static string _connectionString;
        private static IHostingEnvironment _environment;
        private readonly string _insertQuery = "INSERT INTO [dbo].[registered_users] ([id_user], [login], [first_name], [salt], [password], [email], [id_role]) OUTPUT INSERTED.id VALUES(@id_user, @login, @fname, @salt, @pass, @email, @id_role)";
        private readonly string _getUserByNameQuery = "SELECT [id_user], [password], [salt] FROM [dbo].[registered_users] WHERE [login] = @login";

        public UsersRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<UsersRepository> logger)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            int createdId = 0;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("@id_user", user.Id);
                cmd.Parameters.AddWithValue("@login", user.Login);
                cmd.Parameters.AddWithValue("@fname", user.Firstname);
                cmd.Parameters.AddWithValue("@salt", user.Salt);
                cmd.Parameters.AddWithValue("@pass", user.Password);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@id_role", user.Role);

                try
                {
                    connection.Open();
                    createdId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return createdId > 0;
        }
        
        public async Task<User> GetAsync(string login)
        {
            User user = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _getUserByNameQuery;
                cmd.Parameters.AddWithValue("@login", login);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            user = new User(
                                reader.GetGuid(0),
                                reader.GetString(1), //password
                                reader.GetString(2)  //salt
                            );
                        }
                        else
                        {
                            _logger.LogInformation("There is not such login in database.");
                        }
                    }
                }
                catch (Exception e)
                {
                    throw new SmartFridgeException("UnknownError", e.Message);
                }

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return user;
        }
    }
}
