
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{
    public class CountryConfiguration:BaseEntityConfiguration<clsCountry>
    {
        public override void Configure(EntityTypeBuilder<clsCountry> builder)
        {
            base.Configure(builder);

            builder.ToTable("Countries");

            builder.Property(x => x.CountryName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CountryCode).IsRequired().HasMaxLength(10);

            builder.HasIndex(x => x.CountryName).IsUnique();
            builder.HasIndex(x => x.CountryCode).IsUnique();
        }
    }
}
