using Microsoft.AspNetCore.Mvc;
using RedisExchange.Services;
using StackExchange.Redis;
using System.Text;

namespace RedisExchange.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class StringTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        [HttpGet]
        public async Task<IActionResult> Set()
        {
            db.StringSet("ziyaretçi", 100);
            db.StringSet("name", "Kerim Özben");
            db.StringSet("obje", Encoding.UTF8.GetBytes("keroo"));
            db.StringSet("success", true);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            db.StringIncrement("ziyaretçi",1);
            var result = db.StringGet("name");


            if (result.HasValue)
                return Ok(result.ToString());

            return BadRequest();
        }
    }
}
