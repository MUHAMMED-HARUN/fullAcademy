using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Institue;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Institute.Institute
{
    public class InstituteConfiguration : BaseEntityConfiguration<clsInstitute>
    {
        public override void Configure(EntityTypeBuilder<clsInstitute> builder)
        {
            base.Configure(builder);

            builder.ToTable("Institutes");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Notes).HasMaxLength(500);

            builder.HasOne(x => x.Address)
                   .WithMany()
                   .HasForeignKey(x => x.AddressID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
