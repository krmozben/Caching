
using InMemory.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InMemory.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;

        public ProductController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public IActionResult Set()
        {
            if (!_memoryCache.TryGetValue<string>("zaman", out string value))
            {
                MemoryCacheEntryOptions options = new();
                /// Ram den kesinlikle 1 dakikda sonra silecektir
                options.AbsoluteExpiration = DateTime.Now.AddMinutes(1);
                /// eğer en az 5 saniye içerisinde erişilmezse ram den silinir, her erişildiğinde ömrü 5 saniye daha uzar. AbsoluteExpiration tanımladığımız için en fazla 1 dakika sonra kesinlikle silinecektir tabi.
                options.SlidingExpiration = TimeSpan.FromSeconds(5);
                /// ram dolduğunda verinin silinme önceliğini belirtiyoruz
                options.Priority = CacheItemPriority.Normal;
                /// veri silinmeden önce çalışacak metod
                options.RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    Console.WriteLine(key + "-" + value + ": " + reason);
                });

                Product p = new() { Id = 1, Name = "Kalem", Price = 20 };

                _memoryCache.Set<Product>("product:1", p, options);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            if (_memoryCache.TryGetValue<Product>("product:1", out Product value))
            {
                return Ok(value);
            }

            return Ok(false);
        }


        [HttpGet]
        public IActionResult Remove()
        {
            _memoryCache.Remove("product:1");

            return Ok();
        }

        [HttpGet]
        public IActionResult GetOrCreate()
        {
            return Ok(_memoryCache.GetOrCreate<Product>("product:1", entry =>
            {
                return new() { Id = 1, Name = "get or create Kalem", Price = 20 };
            }));
        }
    }
}
