using Microsoft.AspNetCore.Mvc;

namespace MyProxy.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProxyController : ControllerBase
    {
        [HttpGet]
        public IActionResult Anything()
        {
            return Ok("Anything");
        }


    }
}
