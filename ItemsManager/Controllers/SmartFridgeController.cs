using ItemsManager.Domain;
using ItemsManager.HTTPStatusMiddleware;
using Microsoft.AspNetCore.Mvc;
using SmartFridge.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SmartFridge.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class SmartFridgeController : Controller
    {
        private static ISmartFridgeRepository _repository;

        public SmartFridgeController(ISmartFridgeRepository repository)
        {
            _repository = repository;
        }

        //[HttpGet]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FridgeItem))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var list = await _repository.GetAllAsync();
        //    if (list == null)
        //        return NotFound();
        //    return Json(list);
        //}

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FridgeItem))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAsync(int id)
        {
            var list = await _repository.GetAsync(id);
            
            if (list == null)
                return NotFound(new ApiStatus(404, "NotFoundError", "Cant get SmartFridge items."));
            return Json(list);
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(int))]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] FridgeItem item)
        {
            Console.WriteLine(item.Quantity);
            Console.WriteLine(item.Weight);
            if (item == null)
            {
                return BadRequest(new ApiStatus(400, "UnknownError", "Item null error."));
            }
            if (string.IsNullOrEmpty(item.ArticleName))
            {
                return BadRequest(new ApiStatus(400, "ArticleNameError", "ArticleName null error."));
            }
            if (string.IsNullOrEmpty(item.Quantity.ToString()))
            {
                return BadRequest(new ApiStatus(400, "ItemQuantityError", "Item quantity null error."));
            }
            if (string.IsNullOrEmpty(item.Weight.ToString()))
            {
                item.Weight = 1;
                //return BadRequest(new ApiStatus(400, "UnknownError", "Item weight null error."));
            }
            if (string.IsNullOrEmpty(item.CategoryID.ToString()))
            {
                return BadRequest(new ApiStatus(400, "CategoryError", "Item category ID null error."));
            }
            
            item.CreatedAt = DateTime.Now;
            item.ArticleName = item.ArticleName.ToLower();

            int createdId = await _repository.CreateAsync(item);

            if (createdId > 0)
                return Ok(new ApiStatus(200, "Created", createdId.ToString()));
            return BadRequest(new ApiStatus(400, "UnknownError", "Unknown SmartFridgeCreate error."));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (await _repository.DeleteAsync(id))
                return new NoContentResult();

            return BadRequest(new ApiStatus(400, "DeleteError", "Delete SmartFridge item error."));
        }

        //TODO:
        //[Obsolete]
        //[Route("~/api/SmartFridge")]
        //[HttpPut]
        //[ProducesResponseType((int)HttpStatusCode.NoContent)]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        //public async Task<IActionResult> UpdateAsync([FromBody] FridgeItem item)
        //{
        //    //if (item == null)
        //    //{
        //    //    return BadRequest();
        //    //}
        //    //if (string.IsNullOrEmpty(item.ArticleName))
        //    //{
        //    //    return BadRequest();
        //    //}
        //    //if (string.IsNullOrEmpty(item.Quantity.ToString()))
        //    //{
        //    //    return BadRequest();
        //    //}
        //    //if (string.IsNullOrEmpty(item.Weight.ToString()))
        //    //{
        //    //    return BadRequest();
        //    //}
        //    Console.WriteLine("Update not supported");

        //    //if (await _repository.UpdateAsync(item))
        //    //    return new NoContentResult();
        //    return BadRequest();
        //}
    }
}
