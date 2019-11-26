using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Hero.Business.Model;
using Hero.Business.Repository;
using Hero.Domain;
using Hero.Domain.Repository;
using Microsoft.Extensions.Logging;

namespace Hero.Business.Service
{
    public class HeroService : IHeroService
    {
        private readonly ILogger<HeroService> logger;
        private readonly IEntityRepository repository;
        private readonly IQueryableInvoker invoker;

        private readonly Expression<Func<Domain.Entity.Hero, HeroModel>> toModel = x => new HeroModel
        {
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Pseudonym = x.Pseudonym
        };

        private readonly Expression<Func<HeroModel, Domain.Entity.Hero>> toEntity = x => new Domain.Entity.Hero
        {
            //Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Pseudonym = x.Pseudonym
        };

        public HeroService(ILogger<HeroService> logger,
            IEntityRepository repository,
            IQueryableInvoker invoker)
        {
            this.logger = logger;
            this.repository = repository;
            this.invoker = invoker;
        }

        public async Task<HeroModel[]> GetAllAsync(CancellationToken cancellationToken)
        {
            var q = this.repository
                .Query<Domain.Entity.Hero>()
                .Select(this.toModel);
            return await this.invoker
                            .ToArrayAsync(q, cancellationToken)
                            .ConfigureAwait(false);
        }

        public async Task<HeroModel> FindOneAsync(int id, CancellationToken cancellationToken)
        {
            var q = this.repository
                .Query<Domain.Entity.Hero>()
                .Where(x => x.Id == id)
                .Select(this.toModel);
            var model = await this.invoker
                                .FirstOrDefaultAsync(q, cancellationToken)
                                .ConfigureAwait(false);
            return model;
        }

        public async Task<HeroModel> AddAsync(HeroModel model, CancellationToken cancellationToken)
        {
            var entity = this.toEntity.Compile().Invoke(model);
            this.repository.Add(entity);
            await this.repository.SaveAsync(cancellationToken);
            var saved = this.toModel.Compile().Invoke(entity);
            return saved;
        }

        public async Task<bool> TryUpdate(int id, HeroModel model, CancellationToken cancellationToken)
        {
            var q = this.repository
                .Query<Domain.Entity.Hero>()
                .Where(x => x.Id == id);
            var entity = await this.invoker
                                    .FirstOrDefaultAsync(q, cancellationToken)
                                    .ConfigureAwait(false);
            if (entity == null) return false;

            entity.LastName = model.LastName;
            entity.Pseudonym = model.Pseudonym;
            entity.FirstName = model.FirstName;

            await this.repository.SaveAsync(cancellationToken);

            return true;
        }

        public async Task<bool> TryDelete(int id, CancellationToken cancellationToken)
        {
            var q = this.repository
                .Query<Domain.Entity.Hero>()
                .Where(x => x.Id == id);
            var entity = await this.invoker
                                    .FirstOrDefaultAsync(q, cancellationToken)
                                    .ConfigureAwait(false);
            if (entity == null) return false;

            this.repository.Remove(entity);
            await this.repository.SaveAsync(cancellationToken);

            return true;
        }
    }
}
