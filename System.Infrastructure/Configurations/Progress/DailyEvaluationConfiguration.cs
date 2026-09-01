

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Progress
{

    public class DailyEvaluationConfiguration : BaseEntityConfiguration<clsDailyEvaluation>
    {
        public override void Configure(EntityTypeBuilder<clsDailyEvaluation> builder)
        {
            base.Configure(builder);

            builder.ToTable("DailyEvaluations");

            builder.Property(x => x.From).IsRequired();
            builder.Property(x => x.To).IsRequired();
            builder.Property(x => x.Date).IsRequired();
            builder.HasOne(x => x.StudentEnrollment)          // DailyEvaluation له StudentEnrollment واحد
                   .WithMany()                                // StudentEnrollment يمكن أن يكون له عدة DailyEvaluation
                   .HasForeignKey(x => x.StudentEnrollmentID)
                   .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.StageInfo)                  // DailyEvaluation له StageInfo واحد
                 .WithMany()                                // StageInfo يمكن أن يتكرر في عدة DailyEvaluation
                 .HasForeignKey(x => x.StageInfoID)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
