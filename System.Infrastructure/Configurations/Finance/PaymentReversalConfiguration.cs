using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Finance;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Finance
{
    public sealed class PaymentReversalConfiguration : BaseEntityConfiguration<clsPaymentReversal>
    {
        public override void Configure(EntityTypeBuilder<clsPaymentReversal> builder)
        {
            base.Configure(builder);
            builder.ToTable("PaymentReversals");
            builder.Property(reversal => reversal.PaymentID).IsRequired();
            builder.Property(reversal => reversal.Amount).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(reversal => reversal.Reason).IsRequired().HasMaxLength(500);
            builder.Property(reversal => reversal.ReversedBy).IsRequired().HasMaxLength(450);
            builder.Property(reversal => reversal.ReversedAt).IsRequired();
            builder.HasIndex(reversal => reversal.PaymentID).IsUnique();
            builder.HasOne(reversal => reversal.Payment)
                .WithOne(payment => payment.Reversal)
                .HasForeignKey<clsPaymentReversal>(reversal => reversal.PaymentID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
