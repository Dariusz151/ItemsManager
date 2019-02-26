using ItemsManager.Common.Exceptions;
using ItemsManager.HttpResponse;
using ItemsManager.HTTPStatusMiddleware;
using ItemsManager.Users.Commands;
using ItemsManager.Users.Domain;
using ItemsManager.Users.Domain.Repositories;
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
            try
            {
                await _usersService.RegisterAsync(command.Login, command.Email, command.Password, command.Firstname);

                return Ok(new ApiStatus(200, "CreatedUser", "User registered successfully."));
            }
            catch (SmartFridgeException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiStatus(400, ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(new ApiStatus(400, "Unknown error", ex.Message));
            }
        }
    }
}
