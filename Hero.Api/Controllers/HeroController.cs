using System.Threading;
using System.Threading.Tasks;
using Hero.Business.Model;
using Hero.Business.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace Hero.Api.Controllers
{
    /// <summary>
    ///    Hero Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class HeroController : Controller
    {
        private readonly ILogger<HeroController> logger;
        private readonly IHeroService service;

        public HeroController(ILogger<HeroController> logger, IHeroService service)
        {
            this.logger = logger;
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken = default)
        {
            var models = await this.service.GetAllAsync(cancellationToken);
            return this.Json(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken = default)
        {
            var model = await this.service.FindOneAsync(id, cancellationToken);
            if (model == null)
            {
                return this.NotFound();
            }
            return this.Json(model);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]HeroModel value, CancellationToken cancellationToken = default)
        {
            var model = await this.service.AddAsync(value, cancellationToken);
            return this.CreatedAtAction("Get", new { id = model.Id }, model);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody]HeroModel value, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.TryUpdate(id, value, cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }
            return this.Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
        {
            var exists = await this.service.TryDelete(id, cancellationToken);
            if (!exists)
            {
                return this.NotFound();
            }
            return this.Ok();
        }
    }
}
