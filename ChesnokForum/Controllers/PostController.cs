using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChesnokForum.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        [HttpGet("get_all")]
        public IActionResult GetPosts()
        {
            List<string> list = new List<string>() { "sdf", "sdf", "sdf", "sdf" };

            return Ok(list);
        }
    }
}
