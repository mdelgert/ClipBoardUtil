//using Microsoft.AspNetCore.Mvc;

//namespace ClipboardUtil.WebApp1.Controllers
//{
//    public class ConfigController : Controller
//    {
//        public IActionResult Index()
//        {
//            return View();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ClipboardUtil.WebApp1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ConfigController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("settings")]
        public ActionResult<object> GetSettings()
        {
            var setting1 = _configuration["AppSettings:Setting1"];
            var setting2 = _configuration["AppSettings:Setting2"];

            return Ok(new { Setting1 = setting1, Setting2 = setting2 });
        }
    }
}
