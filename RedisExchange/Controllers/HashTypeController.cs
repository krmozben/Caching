using Microsoft.AspNetCore.Mvc;
using RedisExchange.Services;
using StackExchange.Redis;
using System.Text;

namespace RedisExchange.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HashTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "hashList";

        public HashTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(4);
        }

        [HttpGet]
        public async Task<IActionResult> Set()
        {
            db.HashSet(listKey, 100, 11);
            db.HashSet(listKey, "test", "value");
            db.HashSet(listKey, false, true);
            db.HashSet(listKey, Encoding.UTF8.GetBytes("abc"), Encoding.UTF8.GetBytes("vv"));


            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            Dictionary<string, string> lst = new Dictionary<string, string>();

            db.HashGetAll(listKey).ToList().ForEach(x =>
            {
                lst.Add(x.Name.ToString(),x.Value.ToString());
            });

            return Ok(lst);
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            db.HashDelete(listKey, "abc");
            db.HashDelete(listKey, 100);


            return Ok();
        }
    }
}
