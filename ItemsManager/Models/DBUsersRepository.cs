using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Text;




namespace SmartFridge.Models
{
    /*
     * 
     * 
     *     //CASTOWANIE BYTE NA VARBINARY OGARNAC@@@@@
     *     LOGIN CONTROLLER
     * 
     * 
     * */

    public class DBUsersRepository : IUsersRepository
    {
        private static string _connectionString;    // "Server=DESKTOP-U1KKR9S\\SQLEXPRESS;Database=FridgeDB;Trusted_Connection=True;";
        private static IHostingEnvironment _environment;
        private readonly string _insertQuery = "INSERT INTO [dbo].[registered_users] ([login], [first_name], [salt], [password], [email], [phone], [id_role]) OUTPUT INSERTED.id_user VALUES(@login, @fname, CAST(@salt as VARBINARY(50)), CAST(@pass as VARBINARY(MAX)), @email, @phone, @id_role)";

        private readonly string _selectQuery = "SELECT [id_user], [salt], [password] FROM [dbo].[registered_users] WHERE [login] = @login";

        public DBUsersRepository(IConfiguration configuration, IHostingEnvironment environment)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
        }
        
        public async Task<int> RegisterAsync(UserDTO user)
        {
            int createdId = 0;

            var salt = HashHelper.CreateSalt(8);
            var hashedPassword = HashHelper.GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt);
            Console.WriteLine("Register, Salt: " + salt);
            Console.WriteLine("Register, pswd: " + hashedPassword);
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("@login", user.Login);
                cmd.Parameters.AddWithValue("@fname", user.Firstname);
                cmd.Parameters.AddWithValue("@salt",  salt);
                cmd.Parameters.AddWithValue("@pass", hashedPassword);
                cmd.Parameters.AddWithValue("@email", user.Email);
                cmd.Parameters.AddWithValue("@phone", user.Phone);
                cmd.Parameters.AddWithValue("@id_role", user.Role);
                
                try
                {
                    connection.Open();
                    createdId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    Console.WriteLine("CreatedID: " + createdId);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return createdId;
        }

        public async Task<int> LoginAsync(UserDTO user)
        {
            int userID = 0;
            byte[] salt;
            byte[] hashedPass;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _selectQuery;
                cmd.Parameters.AddWithValue("@login", user.Login);
                try
                {
                    connection.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        Console.WriteLine("Count: " + reader.FieldCount);
                        if (reader.HasRows)
                        {
                            reader.Read();
                            userID = (int)reader["id_user"];
                            salt = (byte[])reader["salt"];
                            hashedPass = (byte[])reader["password"];
                            Console.WriteLine("Salt " + salt);
                            Console.WriteLine("hashedPass: " + hashedPass);
                            var hashEnteredPassword = HashHelper.GenerateSaltedHash(Encoding.UTF8.GetBytes(user.Password), salt);
                            Console.WriteLine("Poprawne haslo: " + Convert.ToBase64String(hashedPass));
                            Console.WriteLine("Wpisane haslo: " + Convert.ToBase64String(hashEnteredPassword));
                            if (hashEnteredPassword.SequenceEqual(hashedPass))
                                return userID;
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

            Console.WriteLine("IsSucces from repo: " + userID);
            return userID;
        }
    }
}
