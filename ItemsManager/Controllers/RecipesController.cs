using ItemsManager.Domain;
using ItemsManager.HTTPStatusMiddleware;
using ItemsManager.Models;
using ItemsManager.Models.DTO;
using ItemsManager.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFridge.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.Controllers
{
    [Authorize]
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : Controller
    {
        private static IRecipesRepository _repository;

        public RecipesController(IRecipesRepository repository)
        {
            _repository = repository;
        }
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();

            if (list == null)
                return NotFound(new ApiStatus(404, "RecipesError", "Recipes not found."));
            return Json(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var list = await _repository.GetAsync(id);

            if (list == null)
                return NotFound(new ApiStatus(404, "RecipeError", "Recipe not found."));
            return Json(list);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] RecipeDTO item)
        {
            Console.WriteLine("create async recipe controller");
            Console.WriteLine("item.desc " + item.Description);
            Console.WriteLine("item.name " + item.Name);
            Console.WriteLine("ietm.ingr " + item.Ingredients.ToString());

            if (string.IsNullOrEmpty(item.Name.ToString()))
            {
                return BadRequest(new ApiStatus(400, "ItemNameError", "ItemName is null or empty."));
            }
            if (string.IsNullOrEmpty(item.Ingredients.ToString()))
            {
                return BadRequest(new ApiStatus(400, "IngredientsError", "Ingredients are null or empty."));
            }
            if (string.IsNullOrEmpty(item.Description.ToString()))
            {
                return BadRequest(new ApiStatus(400, "DescriptionError", "Recipe description is null or empty."));
            }

            item.Ingredients.ForEach(x => x.Name = x.Name.ToLower());

            int createdId = await _repository.CreateAsync(item);

            if (createdId > 0)
                return Ok(new ApiStatus(200, "RecipeCreated", "Recipe created successfully."));
            return BadRequest(new ApiStatus(400, "RecipesError", "Recipes Bad Request."));
        }

        //[HttpDelete("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.NoContent)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //public async Task<IActionResult> DeleteAsync(int id)
        //{
        //    //if (await _repository.DeleteAsync(id))
        //    //    return new NoContentResult();

        //    //return BadRequest();
        //}

        //[Obsolete]
        //[Route("~/api/SmartFridge")]
        //[HttpPut]
        //[ProducesResponseType((int)HttpStatusCode.NoContent)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        //public async Task<IActionResult> UpdateAsync([FromBody] FridgeItem item)
        //{

        //    //if (string.IsNullOrEmpty(item.Weight.ToString()))
        //    //{
        //    //    Console.WriteLine("[HttpPut] Weight NullOrEmpty");
        //    //    return BadRequest();
        //    //}

        //    Console.WriteLine("Update not supported");

        //    //if (await _repository.UpdateAsync(item))
        //    //    return new NoContentResult();
        //    return BadRequest();
        //}
    }
}
