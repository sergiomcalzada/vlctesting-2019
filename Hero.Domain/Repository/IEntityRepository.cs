using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hero.Domain.Repository
{
    public interface IEntityRepository
    {
        IQueryable<T> Query<T>() where T : class;
        void Add<T>(T entity) where T : class;
        void Remove<T>(T entity) where T : class;
        Task SaveAsync(CancellationToken cancellationToken);
    }
}
