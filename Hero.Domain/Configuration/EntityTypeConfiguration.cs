using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Hero.Domain.Configuration
{
  public interface IEntityTypeConfiguration
  {
    void Configure(ModelBuilder builder);
  }
  public abstract class EntityTypeConfiguration<TEntity> : IEntityTypeConfiguration, IEntityTypeConfiguration<TEntity>
      where TEntity : class
  {

    public abstract void Configure(EntityTypeBuilder<TEntity> builder);

    public void Configure(ModelBuilder builder)
    {
      builder.ApplyConfiguration(this);
    }
  }

}
