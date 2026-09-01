using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class AssignmentScopeConfiguration : BaseEntityConfiguration<clsAssignmentScope>
    {
        public override void Configure(EntityTypeBuilder<clsAssignmentScope> builder)
        {
            base.Configure(builder);

            builder.ToTable("AssignmentScopes");

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(x => x.Notes)
                .HasMaxLength(500);

            builder.Property(x => x.StageID)
                .IsRequired();

            builder.HasOne(x => x.Stage)
                .WithMany()
                .HasForeignKey(x => x.StageID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.PrevAssignmentScope)
                .WithMany()
                .HasForeignKey(x => x.PrevAssignmentScopeID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
