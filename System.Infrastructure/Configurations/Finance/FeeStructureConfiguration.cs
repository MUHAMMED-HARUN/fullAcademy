using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Finance;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Finance
{
    public class FeeStructureConfiguration : BaseEntityConfiguration<clsFeeStructure>
    {
        public override void Configure(EntityTypeBuilder<clsFeeStructure> builder)
        {
            base.Configure(builder);

            builder.ToTable("FeeStructures");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.FeeType).IsRequired();
            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Notes).IsRequired(false);
            builder.Property(x => x.AcademicPlanID).IsRequired();
            builder.HasIndex(x => new { x.AcademicPlanID, x.FeeType });
            builder.HasOne(x => x.AcademicPlan)
                   .WithMany()
                   .HasForeignKey(x => x.AcademicPlanID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();
        }
    }
}
