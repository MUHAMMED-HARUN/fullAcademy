

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{
    public class NeighborhoodConfiguration:BaseEntityConfiguration<clsNeighborhood>
    {
        public override void Configure(EntityTypeBuilder<clsNeighborhood> builder)
        {
            base.Configure(builder);

            builder.ToTable("Neighborhoods");

            builder.Property(x => x.NeighborhoodName).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.District)
                   .WithMany()
                   .HasForeignKey(x => x.DistrictID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
