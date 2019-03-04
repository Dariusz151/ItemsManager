using ItemsManager.Common.HTTP.Responses;
using ItemsManager.Recipes.Domain;
using ItemsManager.Recipes.DTO;
using ItemsManager.Recipes.Repositories;
using ItemsManager.Recipes.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.Recipes.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FindRecipeController : ControllerBase
    {
        private readonly IRecipesRepository _repository;

        public FindRecipeController(IRecipesRepository repository)
        {
            _repository = repository;
        }
        
        /// <summary>
        /// Gets list of ingredients and returns matched recipes from available.
        /// </summary>
        /// <param name="list"></param>
        /// <returns>List of recipes</returns>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindAsync([FromBody] List<Ingredient> list)
        {
            IEnumerable<RecipeDetailsDTO> recipes = await _repository.GetAllRecipesAsync();

            var matchedRecipes = MatchIngredients.Match(recipes, list);

            if (matchedRecipes != null)
            {
                return Ok(matchedRecipes);
            }

            return BadRequest(new ApiStatus(400, "CheckRecipeError", "Can't find and match any recipe."));
        }
    }
}
