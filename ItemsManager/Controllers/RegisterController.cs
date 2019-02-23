using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartFridge.Models;
using ItemsManager.HTTPStatusMiddleware;
using System.Net;

namespace SmartFridge.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : Controller
    {
        private readonly ILogger<RegisterController> _log;
        private static IUsersRepository _repository;

        public RegisterController(IUsersRepository repository, ILogger<RegisterController> log)
        {
            _repository = repository;
            _log = log;
        }
        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync([FromBody] UserDTO user)
        {
            if (user == null)
            {
                return BadRequest(new ApiStatus(400, "UserNull", "The user object is null."));
            }
            if (string.IsNullOrEmpty(user.Login))
            {
                return BadRequest(new ApiStatus(400, "LoginEmpty", "The login is null or empty."));
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new ApiStatus(400, "PasswordEmpty", "The password is null or empty."));
            }
            if (string.IsNullOrEmpty(user.Firstname))
            {
                return BadRequest(new ApiStatus(400, "FirstnameEmpty", "The first name is null or empty."));
            }
            if (string.IsNullOrEmpty(user.Email))
            {
                user.Email = "none";
            }
            if (string.IsNullOrEmpty(user.Phone))
            {
                user.Phone = "none";
            }

            int createdId = await _repository.RegisterAsync(user);
            //_log.LogInformation("RegisterController, createdID: " + createdId);
            if (createdId > 0)
                return Ok(new ApiStatus(200, "CreatedUser", "User registered successfully."));
            return BadRequest(new ApiStatus(400, "UnknownError", "Unknown bad request error."));
        }
    }
}