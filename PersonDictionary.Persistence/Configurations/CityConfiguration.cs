using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonDictionary.Domain.Entities;

namespace PersonDictionary.Persistence.Configurations
{
    public class CityConfiguration : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.NameKa).HasMaxLength(30);
            builder.Property(x => x.NameEn).HasMaxLength(30);
        }
    }
}