using ItemsManager.Domain;
using ItemsManager.Models.DTO;
using ItemsManager.Models.Helpers;
using ItemsManager.Models.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace ItemsManager.Models.Repositories
{
    public class CheckRecipeRepository : ICheckRecipeRepository
    {
        private readonly ILogger<CheckRecipeRepository> _log;
        private static string _connectionString;
        private static IHostingEnvironment _environment;
        private readonly string _selectAllQuery = "SELECT [recipe_name], [ingredients], [description] FROM [dbo].[recipes]";

        public CheckRecipeRepository(IConfiguration configuration, IHostingEnvironment environment, ILogger<CheckRecipeRepository> log)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
            _log = log;
        }

        public async Task<IEnumerable<RecipeDTO>> GetAllRecipesAsync()
        {
            IList<RecipeDTO> list = new List<RecipeDTO>();

            if (_environment.IsDevelopment())
            {
                Console.WriteLine("(GetAllAsync) in CheckRecipeRepository");
            }
            if (_environment.IsProduction())
            {
                _log.LogInformation("(GetAllAsync) in CheckRecipeRepository");
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
                            while (reader.Read())
                            {
                                try
                                {
                                    list.Add(new RecipeDTO()
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
