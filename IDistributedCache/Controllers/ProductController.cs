using IDistributedCache.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text;
using System.Text.Json;

namespace IDistributedCache.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {
        /// <summary>
        /// IDistributedCache veriyi redis tarafýnda HASH(key-value list) tipinde saklar.
        /// </summary>

        private readonly Microsoft.Extensions.Caching.Distributed.IDistributedCache _distributedCache;

        public WeatherForecastController(Microsoft.Extensions.Caching.Distributed.IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        [HttpGet]
        public IActionResult Set()
        {
            DistributedCacheEntryOptions options = new();
            options.AbsoluteExpiration = DateTime.Now.AddDays(1);

            Product p = new() { Id = 1, Name = "Kalem", Price = 20 };

            _distributedCache.SetString("product:1", JsonSerializer.Serialize(p), options);

            _distributedCache.Set("productByte:1", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(p)));

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _distributedCache.GetStringAsync("product:1");

            Product p = JsonSerializer.Deserialize<Product>(result);

            var resultByte = await _distributedCache.GetAsync("productByte:1");

            Product p1 = JsonSerializer.Deserialize<Product>(Encoding.UTF8.GetString(resultByte));

            return Ok(p1);
        }

        [HttpGet]
        public async Task<IActionResult> Remove()
        {
            await _distributedCache.RemoveAsync("name");

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> SetImage()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Images/img.jpg");

            byte[] img = System.IO.File.ReadAllBytes(path);

            await _distributedCache.SetAsync("resim",img);

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetImage()
        {
            byte[] rsm = _distributedCache.Get("resim");

            return File(rsm,"image/jpg");
        }
    }
}