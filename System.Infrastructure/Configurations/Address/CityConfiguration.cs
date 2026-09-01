using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{
    public class CityConfiguration:BaseEntityConfiguration<clsCity>
    {
        public override void Configure(EntityTypeBuilder<clsCity> builder)
        {
            base.Configure(builder);

            builder.ToTable("Cities");

            builder.Property(x => x.CityName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CityCode).HasMaxLength(10);

            builder.HasOne(x => x.Country)
                   .WithMany()
                   .HasForeignKey(x => x.CountryID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
