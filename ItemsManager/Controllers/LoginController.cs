using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartFridge.Models;

namespace SmartFridge.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private static IUsersRepository _repository;

        public LoginController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] UserDTO user)
        {
            int userID = 0;
            Console.WriteLine("[Controller Login] item: " + user.Login);
            Console.WriteLine("[Controller Login] item: " + user.Password);

            if (user == null)
            {
                Console.WriteLine("[HttpPost Controller Login] Jestem w Item=Null, zwracam BadRequest");
                return BadRequest();
            }
            if (string.IsNullOrEmpty(user.Login))
            {
                Console.WriteLine("[HttpPost Controller Login] Username NullOrEmpty");
                return BadRequest();
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                Console.WriteLine("[HttpPost Controller Login] Password NullOrEmpty");
                return BadRequest();
            }

            userID = _repository.LoginAsync(user).Result;
            Console.WriteLine("Controller Login, isSuccess: " + userID);
            if (userID > 0)
                return Json(userID);
            return BadRequest();
        }
    }
}