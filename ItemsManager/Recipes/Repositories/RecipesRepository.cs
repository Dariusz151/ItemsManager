using ItemsManager.Recipes.Commands;
using ItemsManager.Recipes.Domain;
using ItemsManager.Recipes.DTO;
using ItemsManager.Utilities.XMLHelper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ItemsManager.Recipes.Repositories
{
    public class RecipesRepository : IRecipesRepository
    {
        private readonly ILogger<RecipesRepository> _logger;
        private static string _connectionString;
        private static IHostingEnvironment _environment;
        private readonly string _selectAllQuery = "SELECT [recipe_id], [recipe_name] FROM [dbo].[recipes]";
        private readonly string _selectByIdQuery = "SELECT [recipe_name], [ingredients], [description] FROM [dbo].[recipes] WHERE recipe_id=@id ORDER BY [createdAt] DESC";
        private readonly string _insertQuery = "INSERT INTO [dbo].[recipes] ([recipe_id], [recipe_name], [ingredients], [description]) OUTPUT INSERTED.id VALUES(@param1,@param2,@param3, @param4)";

        private readonly string _selectAllRecipesQuery = "SELECT [recipe_name], [ingredients], [description] FROM [dbo].[recipes]";

        //private readonly string _deleteQuery = "DELETE FROM [dbo].[recipes] WHERE [recipe_id] = @id";

        public RecipesRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<RecipesRepository> log)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _logger = log;
        }

        public async Task<IEnumerable<RecipeDTO>> GetAllAsync()
        {
            IList<RecipeDTO> list = null;
            
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(GetAllAsync) in RecipesRepository");
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
                            list = new List<RecipeDTO>();
                            while (reader.Read())
                            {
                                list.Add(new RecipeDTO()
                                {
                                    Id = reader.GetGuid(0),
                                    Name = reader.GetString(1)
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

        public async Task<RecipeDetailsDTO> GetAsync(Guid id)
        {
            RecipeDetailsDTO recipeDetails = null;
            
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(GetAsync) in RecipesRepository. ID :" + id);
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
                            recipeDetails = new RecipeDetailsDTO();
                            while (reader.Read())
                            {
                                recipeDetails.Name = reader.GetString(0);
                                recipeDetails.Ingredients = reader.GetString(1).XmlDeserializeFromString<List<Ingredient>>();
                                recipeDetails.Description = reader.GetString(2);
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
            return recipeDetails;
        }

        public async Task<bool> CreateAsync(Recipe recipe)
        {
            int createdId = 0;
            
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(CreateAsync) in RecipesRepository ");
            }

            var ingredientsXML = recipe.Ingredients.XmlSerializeToString();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _insertQuery;
                cmd.Parameters.AddWithValue("@param1", recipe.Id);
                cmd.Parameters.AddWithValue("@param2", recipe.Name);
                cmd.Parameters.AddWithValue("@param3", ingredientsXML.ToString());
                cmd.Parameters.AddWithValue("@param4", recipe.Description);
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
        
        public async Task<IEnumerable<RecipeDetailsDTO>> GetAllRecipesAsync()
        {
            IList<RecipeDetailsDTO> list = new List<RecipeDetailsDTO>();
            
            if (_environment.IsProduction())
            {
                _logger.LogInformation("(GetAllAsync) in CheckRecipeRepository");
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = _selectAllRecipesQuery;

                try
                {
                    connection.Open();
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                try
                                {
                                    list.Add(new RecipeDetailsDTO()
                                    {
                                        Name = reader.GetString(0),
                                        Ingredients = reader.GetString(1).XmlDeserializeFromString<List<Ingredient>>(),
                                        Description = reader.GetString(2)
                                    });
                                }
                                catch (InvalidOperationException)
                                {
                                    _logger.LogError("Error in XML document.");
                                }
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
    }
}
