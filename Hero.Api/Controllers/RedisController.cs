using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Hero.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisController : Controller
    {
        private readonly IDistributedCache cache;

        public RedisController(IDistributedCache cache)
        {
            this.cache = cache;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key, CancellationToken cancellationToken = default)
        {
            var value = await this.cache.GetStringAsync(key, cancellationToken);
            if (value == null)
            {
                return this.NotFound();
            }

            return this.Json(new RedisModel()
            {
                Value = value
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RedisModel model, CancellationToken cancellationToken = default)
        {
            var key = Guid.NewGuid().ToString();
            await this.cache.SetStringAsync(key, model.Value, new DistributedCacheEntryOptions()
            {
                AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(60)
            }, cancellationToken);
            return this.CreatedAtAction("Get", new { key = key });
        }

        public class RedisModel
        {
            public string Value { get; set; }
        }
    }
}