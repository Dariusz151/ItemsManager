using ItemsManager.Common.Auth;
using ItemsManager.Common.HTTP.Responses;
using ItemsManager.FoodItems.Domain.Repositories;
using ItemsManager.Recipes.Commands;
using ItemsManager.Recipes.Domain;
using ItemsManager.Recipes.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.Recipes.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesRepository _recipeRepository;
        private readonly IFoodItemsRepository _foodItemsRepository;

        public RecipesController(IRecipesRepository repository, IFoodItemsRepository foodItemsRepository)
        {
            _recipeRepository = repository;
            _foodItemsRepository = foodItemsRepository;
        }

        /// <summary>
        /// Gets all available recipes (no specific user)
        /// </summary>
        /// <returns>List of recipes</returns>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var list = await _recipeRepository.GetAllAsync();

            if (list == null)
                return NotFound(new ApiStatus(404, "RecipesError", "Recipes not found."));
            return Ok(list);
        }

        /// <summary>
        /// Gets details of a specific recipe by Guid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>RecipeDetails</returns>
        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var recipeDetails = await _recipeRepository.GetAsync(id);

            if (recipeDetails == null)
                return NotFound(new ApiStatus(404, "RecipeError", "Recipe not found."));
            return Ok(recipeDetails);
        }

        /// <summary>
        /// Create a new recipe to the database.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>IsCreated (if operation succeeded).</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRecipe command)
        {
            if (string.IsNullOrEmpty(command.Name.ToString()))
            {
                return BadRequest(new ApiStatus(400, "ItemNameError", "ItemName is null or empty."));
            }
            if (string.IsNullOrEmpty(command.Ingredients.ToString()))
            {
                return BadRequest(new ApiStatus(400, "IngredientsError", "Ingredients are null or empty."));
            }
            if (string.IsNullOrEmpty(command.Description.ToString()))
            {
                return BadRequest(new ApiStatus(400, "DescriptionError", "Recipe description is null or empty."));
            }
            
            command.Ingredients.ForEach(x => x.Name = x.Name.ToLower());

            bool isCreated = await _recipeRepository.CreateAsync(
                new Recipe(
                    Guid.NewGuid(),
                    command.Name,
                    command.Description,
                    command.Ingredients
                 ));

            if (isCreated)
                return Ok(new ApiStatus(200, "RecipeCreated", "Recipe created successfully."));
            return BadRequest(new ApiStatus(400, "RecipesError", "Recipes Bad Request."));
        }

        /// <summary>
        /// Consume food items from given recipe.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>IsSucceeded (if operation succeeded).</returns>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ConsumeAsync([FromBody] ConsumeFoodItems command)
        {
            bool isSucceded = false;
            var userId = GuidFromToken.Get(HttpContext);

            if (userId == Guid.Empty)
                return BadRequest(new ApiStatus(400, "UserIdEmpty", "User id is empty."));
            
            if (command.RecipeId != null)
            {
                var foodItems = await _recipeRepository.GetAsync(command.RecipeId);
                if (foodItems != null)
                {
                    isSucceded = await _foodItemsRepository.ConsumeFoodItemsAsync(foodItems.Ingredients, userId);
                }
                else
                {
                    return BadRequest(new ApiStatus(400, "RecipesError", "Recipes Bad Request."));
                }
            }

            if (isSucceded)
                return Ok(new ApiStatus(200, "FoodItemsConsumed", "Food items from recipe consumed."));
            return BadRequest(new ApiStatus(400, "RecipesError", "Recipes id bad request."));
        }
    }
}
