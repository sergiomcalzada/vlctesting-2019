using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hero.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisController : Controller
    {
        private readonly ILogger<RedisController> logger;
        private readonly IDistributedCache cache;
        private readonly IConfiguration configuration;

        public RedisController(ILogger<RedisController> logger, IDistributedCache cache, IConfiguration configuration)
        {
            this.logger = logger;
            this.cache = cache;
            this.configuration = configuration;
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