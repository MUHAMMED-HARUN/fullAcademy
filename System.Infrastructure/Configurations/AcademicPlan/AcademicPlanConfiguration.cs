using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.AcademicPlan;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.AcademicPlan
{
    public class AcademicPlanConfiguration : BaseEntityConfiguration<clsAcademicPlan>
    {
        public override void Configure(EntityTypeBuilder<clsAcademicPlan> builder)
        {
            base.Configure(builder);

            builder.ToTable("AcademicPlans");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
            builder.Property(x => x.ProviderInstituteID).IsRequired();
            builder.Property(x => x.ScopeType).IsRequired();
            builder.Property(x => x.Version).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.EffectiveFrom).IsRequired();
            builder.Property(x => x.EffectiveTo).IsRequired(false);
            builder.Property(x => x.Notes).IsRequired(false).HasMaxLength(1000);

            builder.HasIndex(x => x.ProviderInstituteID);
            builder.HasIndex(x => new { x.ScopeType, x.ProgramID, x.InstituteClassID, x.StageID, x.StageInfoID, x.Status });

            builder.HasOne(x => x.ProviderInstitute)
                   .WithMany()
                   .HasForeignKey(x => x.ProviderInstituteID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired();

            builder.HasOne(x => x.Program)
                   .WithMany()
                   .HasForeignKey(x => x.ProgramID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasOne(x => x.InstituteClass)
                   .WithMany()
                   .HasForeignKey(x => x.InstituteClassID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasOne(x => x.Stage)
                   .WithMany()
                   .HasForeignKey(x => x.StageID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasOne(x => x.StageInfo)
                   .WithMany()
                   .HasForeignKey(x => x.StageInfoID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            builder.HasMany(x => x.Items)
                   .WithOne(x => x.AcademicPlan)
                   .HasForeignKey(x => x.AcademicPlanID)
                   .OnDelete(DeleteBehavior.Cascade)
                   .IsRequired();
        }
    }
}
