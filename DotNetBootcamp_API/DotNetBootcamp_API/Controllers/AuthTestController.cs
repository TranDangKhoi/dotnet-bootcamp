using DotNetBootcamp_API.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBootcamp_API.Controllers
{
    [Route("api/authtest")]
    [ApiController]
    public class AuthTestController : ControllerBase
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<string>> GetSomething()
        {
            return "You're authenticated";
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = SD.Role_Admin)]
        public async Task<ActionResult<string>> GetSomething(int someIntValue)
        {
            // Authorization => Authentication + Some access
            return "You're authenticated with Role of Admin";
        }
    }
}
