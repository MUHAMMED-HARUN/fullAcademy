

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{

    public class StageInfoConfiguration : BaseEntityConfiguration<clsStageInfo>
    {
        public override void Configure(EntityTypeBuilder<clsStageInfo> builder)
        {
            base.Configure(builder);

            builder.ToTable("StageInfos");

            builder.Property(x => x.UniteType).IsRequired();
            builder.Property(x => x.ProgressType).IsRequired();
            builder.Property(x => x.From).IsRequired();
            builder.Property(x => x.To).IsRequired();

            builder.HasOne(x => x.Stage)
                 .WithMany(s=>s.StageInfoItems) // 1-to-1
                 .HasForeignKey(x => x.StageID)
                 .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Matter)
                  .WithMany(st=>st.StageInfoMatters) // 1-to-1
                  .HasForeignKey(x => x.MatterID)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
