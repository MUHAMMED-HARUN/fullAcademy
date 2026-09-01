using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Institute.Institute
{
    public class GroupConfiguration : BaseEntityConfiguration<clsGroup>
    {
        public override void Configure(EntityTypeBuilder<clsGroup> builder)
        {
            base.Configure(builder);

            builder.ToTable("Groups");

            builder.Property(x => x.GroupName).IsRequired().HasMaxLength(150);
            builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
            builder.Property(x => x.InstituteClassID).IsRequired();

            builder.HasOne(x => x.InstituteClass)
                   .WithMany()
                   .HasForeignKey(x => x.InstituteClassID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
