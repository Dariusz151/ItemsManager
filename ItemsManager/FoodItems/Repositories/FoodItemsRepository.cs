using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ItemsManager.Common.XML;
using ItemsManager.FoodItems.Domain.Models;
using ItemsManager.FoodItems.Domain.Repositories;
using ItemsManager.FoodItems.DTO;
using ItemsManager.Recipes.Domain;
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
        private readonly string _insertQuery = "INSERT INTO [dbo].[article] ([article_id], [article_name], [quantity], [weight], [createdAt], [id_user], [id_category]) OUTPUT INSERTED.id VALUES(@param1,@param2,@param3,@param4,@param5,@param6, @param7)";
        private readonly string _deleteQuery = "DELETE FROM [dbo].[article] WHERE [article_id] = @id";

        private readonly string sp_name = "sp_U_ConsumeFoodItemsByList";

        public FoodItemsRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<FoodItemsRepository> logger)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _logger = logger;
        }

        public async Task<IEnumerable<FoodItemDTO>> GetAllAsync()
        {
            IList<FoodItemDTO> list = null;

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
                    connection.Open();
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
                            _logger.LogInformation("No rows found.");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return list;
        }

        public async Task<IEnumerable<FoodItemDTO>> GetAsync(Guid id)
        {
            IList<FoodItemDTO> list = null;

            if (_environment.IsProduction())
            {
                _logger.LogInformation("(GetAsync) in DBFridgeRepos. GUID :" + id);
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
                    connection.Open();
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
                            _logger.LogInformation("No rows found.");
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return list;
        }

        public async Task<bool> CreateAsync(FoodItem foodItem)
        {
            int createdId = 0;

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
                cmd.Parameters.AddWithValue("@param1", foodItem.Id);
                cmd.Parameters.AddWithValue("@param2", foodItem.Name);
                cmd.Parameters.AddWithValue("@param3", foodItem.Quantity);
                cmd.Parameters.AddWithValue("@param4", foodItem.Weight);
                cmd.Parameters.AddWithValue("@param5", foodItem.CreatedAt);
                cmd.Parameters.AddWithValue("@param6", foodItem.UserId);
                cmd.Parameters.AddWithValue("@param7", foodItem.CategoryId);
                try
                {
                    connection.Open();
                    createdId = (int)await cmd.ExecuteScalarAsync();
                    _logger.LogInformation("CreatedID: " + createdId);
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

        public async Task<bool> DeleteAsync(Guid id)
        {
            int rowsAffected = 0;

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
                    _logger.LogInformation("Rows Affected: " + rowsAffected);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return rowsAffected != 0;
        }

        public async Task<bool> ConsumeFoodItemsAsync(List<Ingredient> ingredients, Guid userId)
        {
            int rowsAffected = 0;

            var ingredientsXML = ingredients.XmlSerializeToString();

            if (_environment.IsProduction())
            {
                _logger.LogInformation("(ConsumeFoodItemsAsync) in DBFridgeRepository ");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = sp_name;

                cmd.Parameters.Add(new SqlParameter("@ingredientsXML", ingredientsXML));
                cmd.Parameters.Add(new SqlParameter
                {
                    ParameterName = "@userId",
                    SqlDbType = SqlDbType.UniqueIdentifier,
                    Value = userId
                });

                try
                {
                    connection.Open();
                    SqlDataReader rdr = await cmd.ExecuteReaderAsync();
                    while (rdr.Read())
                    {
                        rowsAffected++;
                    }
                    
                    _logger.LogInformation("Rows Affected: " + rowsAffected);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message.ToString(), "Error Message");
                }
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            return rowsAffected == ingredients.Count;
        }
    }
}
