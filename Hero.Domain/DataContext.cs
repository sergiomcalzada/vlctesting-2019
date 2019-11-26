using Hero.Domain.Configuration;
using Hero.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Hero.Domain
{
    public class DataContext : DbContext
    {
        protected DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            this.LoadConfigurations(modelBuilder);
        }

        private void LoadConfigurations(ModelBuilder builder)
        {
            var configurations = this.GetType()
                                        .GetTypeInfo()
                                        .Assembly.ExportedTypes
                                        .Where(x => typeof(IEntityTypeConfiguration).IsAssignableFrom(x) && !x.GetTypeInfo().IsAbstract)
                                        .ToArray();
            foreach (var c in configurations)
            {
                var config = Activator.CreateInstance(c) as IEntityTypeConfiguration;
                config?.Configure(builder);
            }


        }

        
    }
}
