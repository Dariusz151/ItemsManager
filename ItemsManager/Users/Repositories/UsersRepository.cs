using ItemsManager.Users.Commands;
using ItemsManager.Users.Domain;
using ItemsManager.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemsManager.Users.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ILogger<UsersRepository> _logger;
        private static string _connectionString;
        private static IHostingEnvironment _environment;
        private readonly string _insertQuery = "INSERT INTO [dbo].[registered_users] ([id_user], [login], [first_name], [salt], [password], [email], [phone], [id_role]) OUTPUT INSERTED.id VALUES(@id_user, @login, @fname, CAST(@salt as VARBINARY(50)), CAST(@pass as VARBINARY(MAX)), @email, @phone, @id_role)";
        private readonly string _selectQuery = "SELECT [id_user], [salt], [password] FROM [dbo].[registered_users] WHERE [login] = @login";

        public UsersRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<UsersRepository> logger)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _logger = logger;
        }

        public async Task<bool> RegisterAsync(User user)
        {
            int createdId = 0;
            var salt = HashHelper.CreateSalt(8);
            var hashedPassword = HashHelper.GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt);

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(RegisterAsync) in DBUsersRepos");
            }
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(RegisterAsync) in DBUsersRepos");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("@id_user", user.Id);
                cmd.Parameters.AddWithValue("@login", user.Login);
                cmd.Parameters.AddWithValue("@fname", user.Firstname);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@pass", hashedPassword);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@id_role", user.Role);

                try
                {
                    connection.Open();
                    createdId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return createdId > 0;
        }

        public async Task<Guid> LoginAsync(LoginUser command)
        {
            Guid userID = Guid.Empty;
            Guid userIDFromDB = Guid.Empty;
            byte[] salt;
            byte[] hashedPass;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(LoginAsync) in DBUsersRepos");
            }
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(LoginAsync) in DBUsersRepos");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _selectQuery;
                cmd.Parameters.AddWithValue("@login", command.Login);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            userIDFromDB = (Guid)reader["id_user"];
                            salt = (byte[])reader["salt"];
                            hashedPass = (byte[])reader["password"];
                            var hashEnteredPassword = HashHelper.GenerateSaltedHash(Encoding.UTF8.GetBytes(command.Password), salt);
                            if (hashEnteredPassword.SequenceEqual(hashedPass))
                                userID = userIDFromDB;
                        }
                        else
                        {
                            Console.WriteLine("No rows found.");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString(), "Error Message");
                }

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return userID;
        }
    }
}
