using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ItemsManager.Domain;
using ItemsManager.HTTPStatusMiddleware;
using ItemsManager.HttpResponse;
using ItemsManager.Models.Interfaces;
using ItemsManager.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemsManager.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class CheckRecipeController : ControllerBase
    {
        private static ICheckRecipeRepository _repository;

        public CheckRecipeController(ICheckRecipeRepository repository)
        {
            _repository = repository;
        }
        
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> FindAsync([FromBody] List<Ingredient> list)
        {
            var recipes = await _repository.GetAllRecipesAsync();

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