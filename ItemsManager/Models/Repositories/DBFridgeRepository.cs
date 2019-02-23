using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using ItemsManager.Domain;

namespace SmartFridge.Models
{
    public class DBFridgeRepository : ISmartFridgeRepository
    {
        private readonly ILogger<DBFridgeRepository> _log;
        private static string _connectionString;
        private static IHostingEnvironment _environment;
        private readonly string _selectAllQuery = "SELECT [article_id], [article_name], [quantity], [weight], [id_user], [id_category] FROM [dbo].[article]";
        private readonly string _selectByIdQuery = "SELECT [article_id], [article_name], [quantity], [weight], [id_user], [id_category] FROM [dbo].[article] WHERE id_user=@id";
        private readonly string _insertQuery = "INSERT INTO [dbo].[article] ([article_name], [quantity], [weight], [createdAt], [id_user], [id_category]) OUTPUT INSERTED.article_id VALUES(@param1,@param2,@param3,@param4,@param5,@param6)";
        private readonly string _deleteQuery = "DELETE FROM [dbo].[article] WHERE [article_id] = @id";

        public DBFridgeRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<DBFridgeRepository> log)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _log = log;
        }

        public async Task<IEnumerable<FridgeItem>> GetAllAsync()
        {
            IList<FridgeItem> list = null;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAllAsync) in DBFridgeRepos");
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(GetAllAsync) in DBFridgeRepos");
            }
                
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _selectAllQuery;

                try
                {
                    Console.WriteLine("Try to open connection");
                    connection.Open();
                    Console.WriteLine("Connection.Open();");
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<FridgeItem>();
                            while (reader.Read())
                            {
                                list.Add(new FridgeItem()
                                {
                                    ID = reader.GetInt32(0),
                                    ArticleName = reader.GetString(1),
                                    Quantity = reader.GetInt32(2),
                                    Weight = reader.GetInt32(3),
                                    CategoryID = reader.GetInt32(5)
                                });
                            }
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
            return list;
        }

        public async Task<IEnumerable<FridgeItem>> GetAsync(int id)
        {
            IList<FridgeItem> list = null;
            
            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAsync) in DBFridgeRepos. ID :" + id);
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(GetAsync) in DBFridgeRepos. ID :" + id);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _selectByIdQuery;
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    Console.WriteLine("Try to open connection");
                    connection.Open();
                    Console.WriteLine("Connection.Open();");
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            list = new List<FridgeItem>();
                            while (reader.Read())
                            {
                                list.Add(new FridgeItem()
                                {
                                    ID = reader.GetInt32(0),
                                    ArticleName = reader.GetString(1),
                                    Quantity = reader.GetInt32(2),
                                    Weight = reader.GetInt32(3),
                                    CategoryID = reader.GetInt32(4)
                                });
                            }
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
            return list;
        }

        public async Task<int> CreateAsync(FridgeItem fridgeItem)
        {
            int createdId = 0;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(CreateAsync) in DBFridgeRepository ");
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(CreateAsync) in DBFridgeRepository ");
            }
            
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("@param1", fridgeItem.ArticleName);
                cmd.Parameters.AddWithValue("@param2", fridgeItem.Quantity);
                cmd.Parameters.AddWithValue("@param3", fridgeItem.Weight);
                cmd.Parameters.AddWithValue("@param4", fridgeItem.CreatedAt);
                cmd.Parameters.AddWithValue("@param5", fridgeItem.UserID);
                cmd.Parameters.AddWithValue("@param6", fridgeItem.CategoryID);
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
        
        public async Task<bool> DeleteAsync(int id)
        {
            int rowsAffected = 0;
           
            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(DeleteAsync) in DBFridgeRepository ");
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(DeleteAsync) in DBFridgeRepository ");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _deleteQuery;

                cmd.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    rowsAffected = await cmd.ExecuteNonQueryAsync();
                    Console.WriteLine("Rows Affected: " + rowsAffected);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return rowsAffected != 0;
        }

        //public async Task<bool> UpdateAsync(FridgeItem item)
        //{
        //    int rowsAffected = 0;
        //    if (_environment.IsDevelopment())
        //        Console.WriteLine("(UpdateAsync) in DBFridgeRepository ");

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connection;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = _updateQuery;

        //        cmd.Parameters.AddWithValue("@param1", item.ArticleName);
        //        cmd.Parameters.AddWithValue("@param2", item.Quantity);
        //        cmd.Parameters.AddWithValue("@param3", item.Weight);
        //        cmd.Parameters.AddWithValue("@id", item.ID);
        //        try
        //        {
        //            connection.Open();
        //            rowsAffected = await cmd.ExecuteNonQueryAsync();
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message.ToString(), "Error Message");
        //        }
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();

        //    }
        //    return rowsAffected != 0;
        //}
    }
}
