using ItemsManager.Common.HTTP.Responses;
using ItemsManager.FoodItems.Commands;
using ItemsManager.FoodItems.Domain.Models;
using ItemsManager.FoodItems.Domain.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ItemsManager.Articles.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FoodItemsController : ControllerBase
    {
        private static IFoodItemsRepository _repository;

        public FoodItemsController(IFoodItemsRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FoodItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync()
        {
            Guid id = new Guid();
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                id = Guid.Parse(identity.Name);
            }

            var list = await _repository.GetAsync(id);

            if (list == null)
                return NotFound(new ApiStatus(404, "NotFoundError", "Cant get SmartFridge items."));
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FoodItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        
        public async Task<IActionResult> GetByIdAsync(Guid id)
        {
            var list = await _repository.GetAsync(id);

            if (list == null)
                return NotFound(new ApiStatus(404, "NotFoundError", "Cant get SmartFridge items."));
            return Ok(list);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(int))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFoodItem command)
        {
            if (command is null)
            {
                return BadRequest(new ApiStatus(400, "UnknownError", "Item null error."));
            }
            if (string.IsNullOrEmpty(command.Name))
            {
                return BadRequest(new ApiStatus(400, "ArticleNameError", "ArticleName null error."));
            }
            if (string.IsNullOrEmpty(command.Quantity.ToString()))
            {
                return BadRequest(new ApiStatus(400, "ItemQuantityError", "Item quantity null error."));
            }
            if (string.IsNullOrEmpty(command.Weight.ToString()))
            {
                return BadRequest(new ApiStatus(400, "UnknownError", "Item weight null error."));
            }
            if (string.IsNullOrEmpty(command.CategoryId.ToString()))
            {
                return BadRequest(new ApiStatus(400, "CategoryError", "Item category ID null error."));
            }

            var isCreated = await _repository.CreateAsync(
                new FoodItem(
                    Guid.NewGuid(),
                    command.Name.ToLower(),
                    command.Weight,
                    command.Quantity,
                    command.CategoryId,
                    command.UserId)
                );

            if (isCreated)
                return Ok(new ApiStatus(200, "Created", "Food item created."));
            return BadRequest(new ApiStatus(400, "UnknownError", "Unknown SmartFridgeCreate error."));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(DeleteFoodItem command)
        {
            if (await _repository.DeleteAsync(command.Id))
                return new NoContentResult();

            return BadRequest(new ApiStatus(400, "DeleteError", "Delete SmartFridge item error."));
        }
    }
}

