using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class ResultConfiguration : BaseEntityConfiguration<clsResult>
    {
        public override void Configure(EntityTypeBuilder<clsResult> builder)
        {
            base.Configure(builder);

            builder.ToTable("Results");

            builder.Property(x => x.AssignmentID)
                .IsRequired();

            builder.Property(x => x.AssignmentScopeInfoID)
                .IsRequired();

            builder.Property(x => x.Score)
                .IsRequired();

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.HasOne(x => x.Assignment)
                .WithMany()
                .HasForeignKey(x => x.AssignmentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.AssignmentScopeInfo)
                .WithMany()
                .HasForeignKey(x => x.AssignmentScopeInfoID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
