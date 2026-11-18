using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Finance;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Finance
{
    public class StudentInvoiceConfiguration : BaseEntityConfiguration<clsStudentInvoice>
    {
        public override void Configure(EntityTypeBuilder<clsStudentInvoice> builder)
        {
            base.Configure(builder);

            builder.ToTable("StudentInvoices");

            builder.Property(x => x.IssueDate).IsRequired();
            builder.Property(x => x.DueDate).IsRequired(false);
            builder.Property(x => x.OriginalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.DiscountAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.RemainingAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Notes).IsRequired(false);
            builder.Property(x => x.StudentAcademicEnrollmentID).IsRequired(false);
            builder.Property(x => x.AcademicPlanVersionSnapshot).IsRequired(false);
            builder.Property(x => x.AcademicPlanNameSnapshot).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.FeeStructureNameSnapshot).HasMaxLength(200).IsRequired(false);
            builder.Property(x => x.BillingPeriodStart).IsRequired(false);
            builder.Property(x => x.BillingPeriodEnd).IsRequired(false);
            builder.HasIndex(x => x.StudentAcademicEnrollmentID);
            builder.HasIndex(x => new { x.StudentAcademicEnrollmentID, x.FeeStructureID, x.BillingPeriodStart, x.BillingPeriodEnd })
                .IsUnique()
                .HasFilter("[DeletedAt] IS NULL AND [StudentAcademicEnrollmentID] IS NOT NULL AND [BillingPeriodStart] IS NOT NULL AND [BillingPeriodEnd] IS NOT NULL");

            builder.HasOne(x => x.Student)
                   .WithMany()
                   .HasForeignKey(x => x.StudentID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.FeeStructure)
                   .WithMany(f => f.StudentInvoices)
                   .HasForeignKey(x => x.FeeStructureID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.StudentAcademicEnrollment)
                   .WithMany()
                   .HasForeignKey(x => x.StudentAcademicEnrollmentID)
                   .OnDelete(DeleteBehavior.NoAction)
                   .IsRequired(false);
        }
    }
}
