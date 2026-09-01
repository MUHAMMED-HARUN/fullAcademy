using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Enrollment
{
    public class TeachingAssignmentConfiguration : BaseEntityConfiguration<clsTeachingAssignment>
    {
        public override void Configure(EntityTypeBuilder<clsTeachingAssignment> builder)
        {
            base.Configure(builder);

            builder.ToTable("TeachingAssignments");

            builder.HasOne(x => x.Teacher)
                   .WithMany()
                   .HasForeignKey(x => x.TeacherID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Group)
                   .WithMany()
                   .HasForeignKey(x => x.GroupID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Subject)
                   .WithMany()
                   .HasForeignKey(x => x.SubjectID)
                   .OnDelete(DeleteBehavior.NoAction);

            // Constraint: Teacher cannot teach same Group with same Subject twice
            builder.HasIndex(x => new { x.TeacherID, x.GroupID, x.SubjectID }).IsUnique();
        }
    }
}
