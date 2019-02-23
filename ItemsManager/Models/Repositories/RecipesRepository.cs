using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using ItemsManager.Models.Interfaces;
using System.Text;
using ItemsManager.Models.Helpers;
using Microsoft.Extensions.Logging;
using ItemsManager.Domain;
using ItemsManager.Models.DTO;

namespace ItemsManager.Models.Repositories
{
    public class RecipesRepository : IRecipesRepository
    {
        private readonly ILogger<RecipesRepository> _log;
        private static string _connectionString; 
        private static IHostingEnvironment _environment;
        private readonly string _selectAllQuery = "SELECT [recipe_id], [recipe_name], [createdAt] FROM [dbo].[recipes]";
        private readonly string _selectByIdQuery = "SELECT [recipe_name], [ingredients], [description], [createdAt] FROM [dbo].[recipes] WHERE recipe_id=@id";
        private readonly string _insertQuery = "INSERT INTO [dbo].[recipes] ([recipe_name], [ingredients], [description]) OUTPUT INSERTED.recipe_id VALUES(@param1,@param2,@param3)";
        //private readonly string _deleteQuery = "DELETE FROM [dbo].[recipes] WHERE [recipe_id] = @id";

        public RecipesRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<RecipesRepository> log)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _log = log;
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
                _log.LogInformation("(GetAllAsync) in RecipesRepository");
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
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreatedAt = reader.GetDateTime(2)
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

        public async Task<IEnumerable<RecipeDTO>> GetAsync(int id)
        {
            IList<RecipeDTO> list = null;
           
            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAsync) in RecipesRepository. ID :" + id);
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(GetAsync) in RecipesRepository. ID :" + id);
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
                            list = new List<RecipeDTO>();
                            while (reader.Read())
                            {
                                list.Add(new RecipeDTO()
                                {
                                    Name = reader.GetString(0),
                                    Ingredients = reader.GetString(1).XmlDeserializeFromString<List<Ingredient>>(),
                                    Description = reader.GetString(2),
                                    CreatedAt = reader.GetDateTime(3)
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

        public async Task<int> CreateAsync(RecipeDTO recipe)
        {
            Console.WriteLine("create async recipe repository");
            Console.WriteLine("item.desc " + recipe.Description);
            Console.WriteLine("item.name " + recipe.Name);
            Console.WriteLine("ietm.ingr " + recipe.Ingredients);

            int createdId = 0;
            
            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(CreateAsync) in RecipesRepository ");
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(CreateAsync) in RecipesRepository ");
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

        //public async Task<bool> DeleteAsync(int id)
        //{
        //    int rowsAffected = 0;
        //    if (_environment.IsDevelopment())
        //        Console.WriteLine("(DeleteAsync) in RecipesRepository ");

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connection;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = _deleteQuery;

        //        cmd.Parameters.AddWithValue("@id", id);
        //        try
        //        {
        //            connection.Open();
        //            rowsAffected = await cmd.ExecuteNonQueryAsync();
        //            Console.WriteLine("Rows Affected: " + rowsAffected);
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
