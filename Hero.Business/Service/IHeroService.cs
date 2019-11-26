using System.Threading;
using System.Threading.Tasks;
using Hero.Business.Model;

namespace Hero.Business.Repository
{
    public interface IHeroService
    {
        Task<HeroModel[]> GetAllAsync(CancellationToken cancellationToken);
        Task<HeroModel> FindOneAsync(int id, CancellationToken cancellationToken);
        Task<HeroModel> AddAsync(HeroModel model, CancellationToken cancellationToken);
        Task<bool> TryUpdate(int id, HeroModel model, CancellationToken cancellationToken);
        Task<bool> TryDelete(int id, CancellationToken cancellationToken);
    }
}
