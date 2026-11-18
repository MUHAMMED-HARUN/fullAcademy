using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.AcademicPlan;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.AcademicPlan
{
    public class AcademicPlanItemConfiguration : BaseEntityConfiguration<clsAcademicPlanItem>
    {
        public override void Configure(EntityTypeBuilder<clsAcademicPlanItem> builder)
        {
            base.Configure(builder);

            builder.ToTable("AcademicPlanItems");

            builder.Property(x => x.AcademicPlanID).IsRequired();
            builder.Property(x => x.StageInfoID).IsRequired();

            builder.HasIndex(x => new { x.AcademicPlanID, x.StageInfoID }).IsUnique();

            builder.HasOne(x => x.StageInfo)
                   .WithMany()
                   .HasForeignKey(x => x.StageInfoID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();
        }
    }
}
