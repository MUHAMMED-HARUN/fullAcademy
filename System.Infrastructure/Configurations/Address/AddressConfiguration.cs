using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Address;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{
    public class AddressConfiguration : BaseEntityConfiguration<clsAddress>
    {
        public override void Configure(EntityTypeBuilder<clsAddress> builder)
        {
            base.Configure(builder);
            builder.ToTable("Addresses");

            builder.Property(x => x.Notes).HasMaxLength(500);

            // Relationships
            builder.HasOne(x => x.Country)
                   .WithMany()
                   .HasForeignKey(x => x.CountryID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.City)
                   .WithMany()
                   .HasForeignKey(x => x.CityID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.District)
                   .WithMany()
                   .HasForeignKey(x => x.DistrictID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Neighborhood)
                   .WithMany()
                   .HasForeignKey(x => x.NeighborhoodID)
                   .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
