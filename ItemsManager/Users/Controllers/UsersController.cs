using ItemsManager.HttpResponse;
using ItemsManager.HTTPStatusMiddleware;
using ItemsManager.Users.Commands;
using ItemsManager.Users.Domain;
using ItemsManager.Users.Repositories;
using ItemsManager.Users.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ItemsManager.Users.Controllers
{
    [Produces("application/json")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly ILogger<UsersController> _logger;
        private readonly IUsersService _usersService;

        public UsersController(IUsersRepository repository, ILogger<UsersController> logger, IUsersService usersService)
        {
            _repository = repository;
            _logger = logger;
            _usersService = usersService;
        }

        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("api/[controller]/login")]
        [HttpPost]
        public async Task<IActionResult> LoginAsync([FromBody] LoginUser command)
        {
            Guid userID = Guid.Empty;

            if (command == null)
            {
                return BadRequest(new ApiStatus(400, "ObjectNull", "The object is null."));
            }
            if (string.IsNullOrEmpty(command.Login))
            {
                return BadRequest(new ApiStatus(400, "LoginEmpty", "The login is null or empty."));
            }
            if (string.IsNullOrEmpty(command.Password))
            {
                return BadRequest(new ApiStatus(400, "PasswordEmpty", "The password is null or empty."));
            }

            userID = await _repository.LoginAsync(new LoginUser(command.Login, command.Password));

            if (userID != Guid.Empty)
            {
                Response response = new Response();
                response.StatusCode = 200;
                response.UserID = userID;
                return Ok(response);
            }
            return BadRequest(new ApiStatus(400, "UnknownError", "The unknown error."));
        }
        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("api/[controller]/register")]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(CreateUser command)
        {
            if (command == null)
            {
                return BadRequest(new ApiStatus(400, "UserNull", "The user object is null."));
            }
            if (string.IsNullOrEmpty(command.Login))
            {
                return BadRequest(new ApiStatus(400, "LoginEmpty", "The login is null or empty."));
            }
            if (string.IsNullOrEmpty(command.Password))
            {
                return BadRequest(new ApiStatus(400, "PasswordEmpty", "The password is null or empty."));
            }
            if (string.IsNullOrEmpty(command.Firstname))
            {
                return BadRequest(new ApiStatus(400, "FirstnameEmpty", "The first name is null or empty."));
            }
            if (string.IsNullOrEmpty(command.Email))
            {
                return BadRequest(new ApiStatus(400, "EmailEmpty", "The email is null or empty."));
            }
            if (string.IsNullOrEmpty(command.Phone))
            {
                return BadRequest(new ApiStatus(400, "PhoneEmpty", "The phone number is null or empty."));
            }

            var isRegistered = await _repository.RegisterAsync(
                new User(
                    command.Login,
                    command.Firstname,
                    command.Email,
                    command.Phone,
                    command.Password,
                    command.Role
                ));

            //_log.LogInformation("RegisterController, createdID: " + createdId);
            if (isRegistered)
                return Ok(new ApiStatus(200, "CreatedUser", "User registered successfully."));
            return BadRequest(new ApiStatus(400, "UnknownError", "Unknown bad request error."));
        }
    }
}
