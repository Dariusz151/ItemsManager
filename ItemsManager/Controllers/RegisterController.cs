using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartFridge.Models;

namespace SmartFridge.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private static IUsersRepository _repository;

        public RegisterController(IUsersRepository repository)
        {
            _repository = repository;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetAllAsync()
        //{
        //    var list = new String[]
        //    {
        //        "siema", "siema2", "tu beda wszyscy users"
        //    };
        //    if (list == null)
        //        return NotFound();
        //    return Json(list);
        //}

        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDTO user)
        {
            Console.WriteLine("[Controller] item: " + user.Login);
            Console.WriteLine("[Controller] item: " + user.Firstname);
            Console.WriteLine("[Controller] item: " + user.Email);
            Console.WriteLine("[Controller] item: " + user.Phone);
            Console.WriteLine("[Controller] item: " + user.Password);

            if (user == null)
            {
                Console.WriteLine("[HttpPost RegisterController] Jestem w Item=Null, zwracam BadRequest");
                return BadRequest();
            }
            if (string.IsNullOrEmpty(user.Login))
            {
                Console.WriteLine("[HttpPost RegisterController] Login NullOrEmpty");
                return BadRequest();
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                Console.WriteLine("[HttpPost RegisterController] Password NullOrEmpty");
                return BadRequest();
            }
            
            int createdId = await _repository.RegisterAsync(user);
            Console.WriteLine("RegisterController, createdID: " + createdId);
            if (createdId > 0)
                return Json(createdId);
            return BadRequest();
        }
    }
}