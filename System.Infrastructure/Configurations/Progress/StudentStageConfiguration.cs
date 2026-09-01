

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{

    public class StudentStageConfiguration : BaseEntityConfiguration<clsStudentStage>
    {
        public override void Configure(EntityTypeBuilder<clsStudentStage> builder)
        {
            base.Configure(builder);

            builder.ToTable("StudentStages");

            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.StudentEnrollment)
                 .WithMany(s=>s.StudentStages) // 1-to-1
                 .HasForeignKey(x => x.EnrollmentStudentID)
                 .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Stage)
                  .WithMany(st=>st.StudentStages) // 1-to-1
                  .HasForeignKey(x => x.StageID)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
