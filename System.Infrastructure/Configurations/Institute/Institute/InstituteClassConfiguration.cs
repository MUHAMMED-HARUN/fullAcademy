using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Institue;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Institute.Institute
{
    public class InstituteClassConfiguration : BaseEntityConfiguration<clsInstituteClass>
    {
        public override void Configure(EntityTypeBuilder<clsInstituteClass> builder)
        {
            base.Configure(builder);

            builder.ToTable("InstituteClasses");

            builder.HasOne(x => x.Institute)
                   .WithMany()
                   .HasForeignKey(x => x.InstituteID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Class)
                   .WithMany()
                   .HasForeignKey(x => x.ClassID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Constraint: Cannot repeat Class in same Institute
            builder.HasIndex(x => new { x.InstituteID, x.ClassID }).IsUnique();
        }
    }
}
