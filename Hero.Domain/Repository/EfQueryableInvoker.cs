using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hero.Domain.Repository
{
    public class EfQueryableInvoker : IQueryableInvoker
    {
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> q, CancellationToken cancellationToken)
        {
            return q.ToArrayAsync(cancellationToken);
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> q, CancellationToken cancellationToken)
        {
            return q.FirstOrDefaultAsync(cancellationToken);
        }
    }
}