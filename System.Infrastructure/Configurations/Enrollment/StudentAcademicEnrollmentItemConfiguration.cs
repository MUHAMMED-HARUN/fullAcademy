using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Enrollment;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Enrollment
{
    public class StudentAcademicEnrollmentItemConfiguration : BaseEntityConfiguration<clsStudentAcademicEnrollmentItem>
    {
        public override void Configure(EntityTypeBuilder<clsStudentAcademicEnrollmentItem> builder)
        {
            base.Configure(builder);
            builder.ToTable("StudentAcademicEnrollmentItems");

            builder.Property(x => x.StudentAcademicEnrollmentID).IsRequired();
            builder.Property(x => x.StageInfoID).IsRequired();
            builder.Property(x => x.From).IsRequired();
            builder.Property(x => x.To).IsRequired();
            builder.Property(x => x.StageID).IsRequired();
            builder.Property(x => x.StageName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.MatterID).IsRequired();
            builder.Property(x => x.MatterName).IsRequired().HasMaxLength(200);
            builder.Property(x => x.UniteType).IsRequired();
            builder.Property(x => x.UniteTypeName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.ProgressType).IsRequired();
            builder.Property(x => x.ProgressTypeName).IsRequired().HasMaxLength(100);
            builder.HasIndex(x => new { x.StudentAcademicEnrollmentID, x.StageInfoID }).IsUnique();
        }
    }
}
