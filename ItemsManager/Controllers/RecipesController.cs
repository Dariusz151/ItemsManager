using ItemsManager.Models;
using ItemsManager.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using SmartFridge.Models;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.Controllers
{
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

        //[HttpGet]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FridgeItem))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAllAsync()
        {
            var list = await _repository.GetAllAsync();
            if (list == null)
                return NotFound();
            return Json(list);
        }

        //[HttpGet("{id}")]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(FridgeItem))]
        //[ProducesResponseType((int)HttpStatusCode.NotFound)]
        //public async Task<IActionResult> GetAsync(int id)
        //{
           
        //    //var list = await _repository.GetAsync(id);

        //    //if (list == null)
        //    //    return NotFound();
        //    //return Json(list);
        //}

        //[HttpPost]
        //[ProducesResponseType((int)HttpStatusCode.OK, Type = typeof(int))]
        //[ProducesResponseType((int)HttpStatusCode.BadRequest)]
        //[ProducesResponseType((int)HttpStatusCode.MethodNotAllowed)]
        //public async Task<IActionResult> CreateAsync([FromBody] Recipe item)
        //{
            
        //    //if (string.IsNullOrEmpty(item.CategoryID.ToString()))
        //    //{
        //    //    Console.WriteLine("[HttpPost] CategoryID NullOrEmpty");
        //    //    return BadRequest();
        //    //}

        //    //item.CreatedAt = DateTime.Now;

        //    //int createdId = await _repository.CreateAsync(item);
           
        //    //if (createdId > 0)
        //    //    return Json(createdId);
        //    //return BadRequest();
        //}

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
