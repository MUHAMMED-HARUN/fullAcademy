using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Enrollment;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Enrollment
{
    public class StudentAcademicEnrollmentConfiguration : BaseEntityConfiguration<clsStudentAcademicEnrollment>
    {
        public override void Configure(EntityTypeBuilder<clsStudentAcademicEnrollment> builder)
        {
            base.Configure(builder);
            builder.ToTable("StudentAcademicEnrollments");

            builder.Property(x => x.StudentID).IsRequired();
            builder.Property(x => x.AcademicPlanID).IsRequired();
            builder.Property(x => x.AcademicPlanVersion).IsRequired();
            builder.Property(x => x.EnrollmentDate).IsRequired();
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.ValidFrom).IsRequired();
            builder.Property(x => x.ValidTo).IsRequired(false);
            builder.HasIndex(x => new { x.StudentID, x.Status });
            builder.HasIndex(x => new { x.AcademicPlanID, x.AcademicPlanVersion });

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentID)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            builder.HasOne(x => x.AcademicPlan)
                .WithMany()
                .HasForeignKey(x => x.AcademicPlanID)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
            builder.HasMany(x => x.Items)
                .WithOne(x => x.StudentAcademicEnrollment)
                .HasForeignKey(x => x.StudentAcademicEnrollmentID)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.HasMany(x => x.GroupEnrollments)
                .WithOne(x => x.StudentAcademicEnrollment)
                .HasForeignKey(x => x.StudentAcademicEnrollmentID)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();
        }
    }
}
