

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Progress;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Progress
{

    public class StageConfiguration : BaseEntityConfiguration<clsStage>
    {
        public override void Configure(EntityTypeBuilder<clsStage> builder)
        {
            base.Configure(builder);

            builder.ToTable("Stages");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Notes).HasMaxLength(500);
            builder.Property(x => x.PrevStageID).HasColumnName("PrevClassID");
            builder.HasOne(x => x.PrevStage)
                 .WithMany(x => x.NextStages)
                 .HasForeignKey(x => x.PrevStageID)
                 .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Class)
                  .WithMany()
                  .HasForeignKey(x => x.ClassID)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
