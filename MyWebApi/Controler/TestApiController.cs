using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Controler
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestApiController : Controller
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok(new {message = "front dziala"});
        }
    }
}
