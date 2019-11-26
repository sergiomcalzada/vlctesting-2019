using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Hero.Domain.Repository;

namespace Hero.Business.Test.Support
{
    public class TestQueryableInvoker : IQueryableInvoker
    {
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> q, CancellationToken cancellationToken)
        {
            return Task.FromResult(q.ToArray());
        }

        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> q, CancellationToken cancellationToken)
        {
            return Task.FromResult(q.FirstOrDefault());
        }
    }
}