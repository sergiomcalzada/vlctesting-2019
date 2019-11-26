using Hero.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hero.Domain.Configuration
{
  public class HeroConfiguration : EntityTypeConfiguration<Entity.Hero>
  {
    public override void Configure(EntityTypeBuilder<Entity.Hero> builder)
    {
      builder.ToTable("Hero");
      builder.HasKey(x => x.Id);
    }
  }
}
