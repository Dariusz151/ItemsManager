using System;
using System.Threading.Tasks;
using ItemsManager.HTTPStatusMiddleware;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartFridge.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using ItemsManager.HttpResponse;
using ItemsManager.Domain;
using System.Net;

namespace SmartFridge.Controllers
{
    /*
     * TODO:
     * HANDLE EXISTING USERS IN DATABASE!!!
     * 
     * */
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private static IUsersRepository _repository;
        private readonly ILogger<LoginController> _log;

        public LoginController(IUsersRepository repository, ILogger<LoginController> log)
        {
            _repository = repository;
            _log = log;
        }
        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] UserDTO user)
        {
            int userID = 0;
           
            if (user == null)
            {
                return BadRequest(new ApiStatus(400, "ObjectNull", "The object is null."));
            }
            if (string.IsNullOrEmpty(user.Login))
            {
                return BadRequest(new ApiStatus(400, "LoginEmpty", "The login is null or empty."));
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                return BadRequest(new ApiStatus(400, "PasswordEmpty", "The password is null or empty."));
            }

            userID = await _repository.LoginAsync(user);
            
            if (userID > 0)
            {
                Response response = new Response();
                response.StatusCode = 200;
                response.UserID = userID;
                return Ok(response);
            }
            return BadRequest(new ApiStatus(400, "UnknownError", "The unknown error."));
        }
    }
}