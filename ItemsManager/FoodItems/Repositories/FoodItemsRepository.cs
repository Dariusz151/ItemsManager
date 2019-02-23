using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ItemsManager.Articles.Domain;
using ItemsManager.FoodItems.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ItemsManager.FoodItems.Repositories
{
    public class FoodItemsRepository : IFoodItemsRepository
    {
        private readonly ILogger<FoodItemsRepository> _logger;
        private static string _connectionString;
        private static IHostingEnvironment _environment;
        private readonly string _selectAllQuery = "SELECT [article_id], [article_name], [quantity], [weight], [id_user], [id_category] FROM [dbo].[article]";
        private readonly string _selectByIdQuery = "SELECT [article_id], [article_name], [quantity], [weight], [id_user], [id_category] FROM [dbo].[article] WHERE id_user=@id";
        private readonly string _insertQuery = "INSERT INTO [dbo].[article] ([article_name], [quantity], [weight], [createdAt], [id_user], [id_category]) OUTPUT INSERTED.article_id VALUES(@param1,@param2,@param3,@param4,@param5,@param6)";
        private readonly string _deleteQuery = "DELETE FROM [dbo].[article] WHERE [article_id] = @id";

        public FoodItemsRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<FoodItemsRepository> logger)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _logger = logger;
        }

        public async Task<IEnumerable<FoodItemDTO>> GetAllAsync()
        {
            IList<FoodItemDTO> list = null;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAllAsync) in DBFridgeRepos");
            }
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(GetAllAsync) in DBFridgeRepos");
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
                            list = new List<FoodItemDTO>();
                            while (reader.Read())
                            {
                                list.Add(new FoodItemDTO()
                                {
                                    Id = reader.GetGuid(0),
                                    Name = reader.GetString(1),
                                    Weight = reader.GetInt32(3),
                                    Quantity = reader.GetInt32(2)
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

        public async Task<IEnumerable<FoodItemDTO>> GetAsync(int id)
        {
            IList<FoodItemDTO> list = null;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAsync) in DBFridgeRepos. ID :" + id);
            }
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(GetAsync) in DBFridgeRepos. ID :" + id);
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
                            list = new List<FoodItemDTO>();
                            while (reader.Read())
                            {
                                list.Add(new FoodItemDTO()
                                {
                                    Id = reader.GetGuid(0),
                                    Name = reader.GetString(1),
                                    Quantity = reader.GetInt32(2),
                                    Weight = reader.GetInt32(3)
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

        public async Task<int> CreateAsync(FoodItem foodItem)
        {
            int createdId = 0;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(CreateAsync) in DBFridgeRepository ");
            }
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(CreateAsync) in DBFridgeRepository ");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("@param1", foodItem.Name);
                cmd.Parameters.AddWithValue("@param2", foodItem.Quantity);
                cmd.Parameters.AddWithValue("@param3", foodItem.Weight);
                cmd.Parameters.AddWithValue("@param4", foodItem.CreatedAt);
                cmd.Parameters.AddWithValue("@param5", foodItem.UserId);
                cmd.Parameters.AddWithValue("@param6", foodItem.CategoryId);
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            int rowsAffected = 0;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(DeleteAsync) in DBFridgeRepository ");
            }
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(DeleteAsync) in DBFridgeRepository ");
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
    }
}
