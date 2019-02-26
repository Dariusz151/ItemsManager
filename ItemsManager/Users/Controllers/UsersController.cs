using ItemsManager.Common.Auth;
using ItemsManager.Common.Exceptions;
using ItemsManager.HTTPStatusMiddleware;
using ItemsManager.Users.Commands;
using ItemsManager.Users.Domain.Repositories;
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
            JsonWebToken token = new JsonWebToken();
            
            try
            {
                token = await _usersService.LoginAsync(command.Login, command.Password);
            }
            catch (SmartFridgeException ex)
            {
                return BadRequest(new ApiStatus(400, ex.Code, ex.Message));
            }
            
            return Ok(token);
        }
        
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
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
                return BadRequest(new ApiStatus(400, ex.Code, ex.Message));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiStatus(400, "Unknown error", ex.Message));
            }
        }
    }
}
