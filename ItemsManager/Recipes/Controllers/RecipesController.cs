using ItemsManager.HTTPStatusMiddleware;
using ItemsManager.Recipes.Commands;
using ItemsManager.Recipes.Domain;
using ItemsManager.Recipes.DTO;
using ItemsManager.Recipes.Repositories;
using ItemsManager.Utilities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.Recipes.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipesRepository _repository;

        public RecipesController(IRecipesRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();

            if (list == null)
                return NotFound(new ApiStatus(404, "RecipesError", "Recipes not found."));
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var list = await _repository.GetAsync(id);

            if (list == null)
                return NotFound(new ApiStatus(404, "RecipeError", "Recipe not found."));
            return Ok(list);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRecipe command)
        {
            Console.WriteLine("create async recipe controller");
            Console.WriteLine("item.desc " + command.Description);
            Console.WriteLine("item.name " + command.Name);
            Console.WriteLine("ietm.ingr " + command.Ingredients.ToString());

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

            Guid createdId = await _repository.CreateAsync(
                new Recipe(
                    new Guid(),
                    command.Name,
                    command.Description,
                    command.Ingredients
                 ));

            if (createdId != Guid.Empty)
                return Ok(new ApiStatus(200, "RecipeCreated", "Recipe created successfully."));
            return BadRequest(new ApiStatus(400, "RecipesError", "Recipes Bad Request."));
        }

        [HttpPost]
        [Route("api/[controller]/find")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindAsync([FromBody] List<Ingredient> list)
        {
            IEnumerable<RecipeDetailsDTO> recipes = await _repository.GetAllRecipesAsync();
            
            //Console.WriteLine("recipes:" + recipes.Count());
            var matchedRecipes = MatchIngredients.Match(recipes, list);

            if (matchedRecipes != null)
            {
                return Ok(matchedRecipes);
            }

            return BadRequest(new ApiStatus(400, "CheckRecipeError", "Can't find and match any recipe."));
        }

    }
}
