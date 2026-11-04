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
            builder.Property(x => x.DueDate).IsRequired();
            builder.Property(x => x.OriginalAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.DiscountAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.RemainingAmount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Status).IsRequired();
            builder.Property(x => x.Notes).IsRequired(false);

            builder.HasOne(x => x.Student)
                   .WithMany()
                   .HasForeignKey(x => x.StudentID)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.FeeStructure)
                   .WithMany(f => f.StudentInvoices)
                   .HasForeignKey(x => x.FeeStructureID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
