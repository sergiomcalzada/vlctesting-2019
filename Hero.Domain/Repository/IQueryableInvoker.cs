using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hero.Domain.Repository
{
    public interface IQueryableInvoker
    {
        Task<T[]> ToArrayAsync<T>(IQueryable<T> q, CancellationToken cancellationToken);
        Task<T> FirstOrDefaultAsync<T>(IQueryable<T> q, CancellationToken cancellationToken);
    }
}