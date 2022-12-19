using Microsoft.AspNetCore.Mvc;
using RedisExchange.Services;
using StackExchange.Redis;
using System.Text;

namespace RedisExchange.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SetTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        string listKey = "setKey";

        public SetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(2);
        }

        [HttpGet]
        public async Task<IActionResult> Set()
        {
            db.KeyExpire(listKey, DateTime.Now.AddMinutes(5));

            db.SetAdd(listKey, 100);
            db.SetAdd(listKey, "test");
            db.SetAdd(listKey, false);
            db.SetAdd(listKey, Encoding.UTF8.GetBytes("abc"));


            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            HashSet<string> lst = new HashSet<string>();

            db.SetMembers(listKey).ToList().ForEach(x =>
            {
                    lst.Add(x.ToString());
            });

            return Ok(lst);
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            db.SetRemove(listKey, "abc");
            db.SetRemove(listKey, 100);


            return Ok();
        }
    }
}
