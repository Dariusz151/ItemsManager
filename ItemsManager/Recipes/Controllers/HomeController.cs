using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ItemsManager.Recipes.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("api/recipes/[controller]")]
        public Task<OkObjectResult> HelloSync()
        {
            return Task.FromResult(Ok("Hello, recipes"));
        }
    }
}
