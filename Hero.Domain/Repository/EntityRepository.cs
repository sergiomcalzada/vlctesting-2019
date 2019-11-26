using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Hero.Domain.Repository
{
    public class EntityRepository : IEntityRepository
    {
        private readonly DataContext dbContext;

        public EntityRepository(DataContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> Query<T>() where T : class
        {
            return this.dbContext.Set<T>();
        }

        public void Add<T>(T entity) where T : class
        {
            this.dbContext.Set<T>().Add(entity);
        }

        public void Remove<T>(T entity) where T : class
        {
            this.dbContext.Set<T>().Remove(entity);
        }

        public async Task SaveAsync(CancellationToken cancellationToken)
        {
            await this.dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}