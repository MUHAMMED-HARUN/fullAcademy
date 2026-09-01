
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{
    public class DistrictConfiguration:BaseEntityConfiguration<clsDistrict>
    {
        public override void Configure(EntityTypeBuilder<clsDistrict> builder)
        {
            base.Configure(builder);

            builder.ToTable("Districts");

            builder.Property(x => x.DistrictName).IsRequired().HasMaxLength(100);

            builder.HasOne(x => x.City)
                   .WithMany()
                   .HasForeignKey(x => x.CityID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
