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

            // Optional FK: ProgramID
            builder.HasOne(x => x.Program)
                   .WithMany()
                   .HasForeignKey(x => x.ProgramID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            // Optional FK: ClassID
            builder.HasOne(x => x.Class)
                   .WithMany()
                   .HasForeignKey(x => x.ClassID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            // Optional FK: StageID
            builder.HasOne(x => x.Stage)
                   .WithMany()
                   .HasForeignKey(x => x.StageID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);

            // Optional FK: StageInfoID
            builder.HasOne(x => x.StageInfo)
                   .WithMany()
                   .HasForeignKey(x => x.StageInfoID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);
        }
    }
}
