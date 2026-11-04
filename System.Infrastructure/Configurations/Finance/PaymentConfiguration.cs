using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Finance;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Finance
{
    public class PaymentConfiguration : BaseEntityConfiguration<clsPayment>
    {
        public override void Configure(EntityTypeBuilder<clsPayment> builder)
        {
            base.Configure(builder);

            builder.ToTable("Payments");

            builder.Property(x => x.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.PaymentDate).IsRequired();
            builder.Property(x => x.PaymentMethod).IsRequired();
            builder.Property(x => x.ReferenceNumber).IsRequired(false).HasMaxLength(100);
            builder.Property(x => x.Notes).IsRequired(false);

            builder.HasOne(x => x.StudentInvoice)
                   .WithMany(i => i.Payments)
                   .HasForeignKey(x => x.StudentInvoiceID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
