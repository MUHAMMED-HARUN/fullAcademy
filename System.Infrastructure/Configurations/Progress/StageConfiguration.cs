

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{

    public class StageConfiguration : BaseEntityConfiguration<clsStage>
    {
        public override void Configure(EntityTypeBuilder<clsStage> builder)
        {
            base.Configure(builder);

            builder.ToTable("Stages");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Notes).HasMaxLength(500);
            builder.HasOne(x => x.PrevStage)
                 .WithOne() // 1-to-1
                 .HasForeignKey<clsStage>(x => x.Id)
                 .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Program)
                  .WithOne() // 1-to-1
                  .HasForeignKey<clsStage>(x => x.ProgramID)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
