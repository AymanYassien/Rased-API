using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Rased.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RasedController : ControllerBase
    {
        [HttpGet("Greet")]
        public IActionResult Get()
        {
            return Ok(new { Message = "Welcome To Rased - رَاصِــــــد" });
        }
    }
}
