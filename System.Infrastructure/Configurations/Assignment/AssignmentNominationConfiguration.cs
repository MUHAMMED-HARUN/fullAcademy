using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class AssignmentNominationConfiguration : BaseEntityConfiguration<clsAssignmentNomination>
    {
        public override void Configure(EntityTypeBuilder<clsAssignmentNomination> builder)
        {
            base.Configure(builder);

            builder.ToTable("AssignmentNominations");

            builder.Property(x => x.StudentEnrollmentID)
                .IsRequired();

            builder.Property(x => x.AssignmentScopeID)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();

            builder.Property(x => x.NominatedByID)
                .IsRequired();

            builder.Property(x => x.NominatedByType)
                .IsRequired();

            builder.Property(x => x.NominationState)
                .IsRequired();

            builder.HasOne(x => x.StudentEnrollment)
                .WithMany()
                .HasForeignKey(x => x.StudentEnrollmentID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.AssignmentScope)
                .WithMany()
                .HasForeignKey(x => x.AssignmentScopeID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
