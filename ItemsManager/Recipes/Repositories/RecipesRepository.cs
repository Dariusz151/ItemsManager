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
        private readonly string _insertQuery = "INSERT INTO [dbo].[recipes] ([recipe_name], [ingredients], [description]) OUTPUT INSERTED.recipe_id VALUES(@param1,@param2,@param3)";

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

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAllAsync) in RecipesRepository");
            }
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
                    Console.WriteLine("Try to open connection");
                    connection.Open();
                    Console.WriteLine("Connection.Open();");
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

        public async Task<RecipeDetailsDTO> GetAsync(Guid id)
        {
            RecipeDetailsDTO recipe = null;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAsync) in RecipesRepository. ID :" + id);
            }
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
                    Console.WriteLine("Try to open connection");
                    connection.Open();
                    Console.WriteLine("Connection.Open();");

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (reader.HasRows)
                        {
                            recipe = new RecipeDetailsDTO();
                            while (reader.Read())
                            {
                                recipe.Name = reader.GetString(0);
                                recipe.Ingredients = reader.GetString(1).XmlDeserializeFromString<List<Ingredient>>();
                                recipe.Description = reader.GetString(2);
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
            return recipe;
        }

        public async Task<Guid> CreateAsync(Recipe recipe)
        {
            Console.WriteLine("Create async recipe repository");
            Console.WriteLine("item.desc " + recipe.Description);
            Console.WriteLine("item.name " + recipe.Name);
            Console.WriteLine("ietm.ingr " + recipe.Ingredients);

            Guid createdId = Guid.Empty;

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(CreateAsync) in RecipesRepository ");
            }
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
                cmd.Parameters.AddWithValue("@param1", recipe.Name);
                cmd.Parameters.AddWithValue("@param2", ingredientsXML.ToString());
                cmd.Parameters.AddWithValue("@param3", recipe.Description);
                try
                {
                    connection.Open();
                    createdId = (Guid)await cmd.ExecuteScalarAsync();
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
        
        public async Task<IEnumerable<RecipeDetailsDTO>> GetAllRecipesAsync()
        {
            IList<RecipeDetailsDTO> list = new List<RecipeDetailsDTO>();

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAllAsync) in CheckRecipeRepository");
            }
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
                    Console.WriteLine("Try to open connection");
                    connection.Open();
                    Console.WriteLine("Connection.Open();");
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
                                    Console.WriteLine("Error in XML document.");
                                }
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
    }
}
