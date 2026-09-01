using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Enrollment
{
    public class StudentEnrollmentConfiguration : BaseEntityConfiguration<clsStudentEnrollment>
    {
        public override void Configure(EntityTypeBuilder<clsStudentEnrollment> builder)
        {
            base.Configure(builder);

            builder.ToTable("StudentEnrollments");

            builder.Property(x => x.StudentID).IsRequired();
            builder.Property(x => x.GroupID).IsRequired();
            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.Student)
                   .WithMany()
                   .HasForeignKey(x => x.StudentID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.Group)
                   .WithMany()
                   .HasForeignKey(x => x.GroupID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
