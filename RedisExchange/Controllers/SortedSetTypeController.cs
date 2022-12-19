using Microsoft.AspNetCore.Mvc;
using RedisExchange.Services;
using StackExchange.Redis;
using System.Text;

namespace RedisExchange.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class SortedSetTypeController : ControllerBase
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;
        private string listKey = "sortedList";
        public SortedSetTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(3);
        }

        [HttpGet]
        public async Task<IActionResult> Set()
        {
            db.SortedSetAdd(listKey, 100,1);
            db.SortedSetAdd(listKey, "test",2);
            db.SortedSetAdd(listKey, false,3);
            db.SortedSetAdd(listKey, Encoding.UTF8.GetBytes("abc"),4);


            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Show()
        {
            HashSet<string> lst = new HashSet<string>();

            db.SortedSetScan(listKey).ToList().ForEach(x =>
            {
                lst.Add(x.ToString());
            });

            return Ok(lst);
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            db.SortedSetRemove(listKey, "abc");
            db.SortedSetRemove(listKey, 100);


            return Ok();
        }
    }
}
