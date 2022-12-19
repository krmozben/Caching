using Microsoft.AspNetCore.Mvc;
using RedisExchange.Services;
using StackExchange.Redis;
using System.Text;

namespace RedisExchange.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ListTypeController : ControllerBase
    {

        private readonly RedisService _redisService;
        private readonly IDatabase db;

        string listKey = "listKey";

        public ListTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(1);
        }
        [HttpGet]
        public async Task<IActionResult> Set()
        {
            db.ListLeftPush(listKey, 100);
            db.ListLeftPush(listKey, "Kerim Özben");
            db.ListLeftPush(listKey, Encoding.UTF8.GetBytes("keroo"));
            db.ListLeftPush(listKey, false);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            List<object> lst = new List<object>();


            if (db.KeyExists(listKey))
            {
                db.ListRange(listKey).ToList().ForEach(x =>
                {
                    lst.Add(x.ToString());

                });

                return Ok(lst);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            db.ListRemove(listKey, false);
            db.ListRemove(listKey, 100);

            /// veya 

            db.ListRightPop(listKey);

            return Ok();
        }
    }
}
