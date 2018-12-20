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

namespace ItemsManager.Models.Repositories
{
    public class RecipesRepository : IRecipesRepository
    {
        private static string _connectionString;    // "Server=DESKTOP-U1KKR9S\\SQLEXPRESS;Database=FridgeDB;Trusted_Connection=True;";
        private static IHostingEnvironment _environment;
        private readonly string _selectAllQuery = "SELECT [recipe_id], [recipe_name], [createdAt] FROM [dbo].[recipes]";
        private readonly string _selectByIdQuery = "SELECT [recipe_name], [ingredients], [description], [createdAt] FROM [dbo].[recipes] WHERE recipe_id=@id";
        private readonly string _insertQuery = "INSERT INTO [dbo].[recipes] ([recipe_name], [ingredients], [description]) OUTPUT INSERTED.recipe_id VALUES(@param1,@param2,@param3)";
        private readonly string _deleteQuery = "DELETE FROM [dbo].[recipes] WHERE [recipe_id] = @id";
        //private readonly string _updateQuery = "UPDATE Articles SET [ArticleName] = @param1, [Quantity] = @param2, [Weight] = @param3 WHERE ID=@id";

        public RecipesRepository(IConfiguration configuration, IHostingEnvironment environment)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _environment = environment;
        }

        public async Task<IEnumerable<Recipe>> GetAllAsync()
        {
            IList<Recipe> list = null;
            if (_environment.IsDevelopment())
                Console.WriteLine("(GetAllAsync) in RecipesRepository");

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
                            list = new List<Recipe>();
                            while (reader.Read())
                            {
                                list.Add(new Recipe()
                                {
                                    ID = reader.GetInt32(0),
                                    Name = reader.GetString(1),
                                    CreatedAt = reader.GetDateTime(2)
                                    //ID = reader.GetInt32(0),
                                    //ArticleName = reader.GetString(1),
                                    //Quantity = reader.GetInt32(2),
                                    //Weight = reader.GetInt32(3),
                                    //CategoryID = reader.GetInt32(5)
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

        //public async Task<IEnumerable<Recipe>> GetAsync(int id)
        //{
        //    IList<Recipe> list = null;
        //    if (_environment.IsDevelopment())
        //        Console.WriteLine("(GetAsync) in RecipesRepository. ID :" + id);

        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    using (SqlCommand cmd = new SqlCommand())
        //    {
        //        cmd.Connection = connection;
        //        cmd.CommandType = CommandType.Text;
        //        cmd.CommandText = _selectByIdQuery;
        //        cmd.Parameters.AddWithValue("@id", id);

        //        try
        //        {
        //            Console.WriteLine("Try to open connection");
        //            connection.Open();
        //            Console.WriteLine("Connection.Open();");
        //            using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //            {
        //                if (reader.HasRows)
        //                {
        //                    list = new List<Recipe>();
        //                    while (reader.Read())
        //                    {
        //                        list.Add(new Recipe()
        //                        {
        //                            //ID = reader.GetInt32(0),
        //                            //ArticleName = reader.GetString(1),
        //                            //Quantity = reader.GetInt32(2),
        //                            //Weight = reader.GetInt32(3),
        //                            //CategoryID = reader.GetInt32(4)
        //                        });
        //                    }
        //                }
        //                else
        //                {
        //                    Console.WriteLine("No rows found.");
        //                }
        //            }
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message.ToString(), "Error Message");
        //        }
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //    }
        //    return list;
        //}

        public async Task<int> CreateAsync(Recipe recipe)
        {
            int createdId = 0;
            if (_environment.IsDevelopment())
                Console.WriteLine("(CreateAsync) in RecipesRepository ");

            Console.WriteLine(recipe.Description);
            Console.WriteLine(recipe.Ingredients);
            Console.WriteLine(recipe.Name);

            var ingredientsXML = recipe.Ingredients.XmlSerializeToString();
            Console.WriteLine(ingredientsXML);

            var ingredientsString = ingredientsXML.XmlDeserializeFromString<List<Ingredient>>();
            Console.WriteLine(ingredientsString);

            
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
