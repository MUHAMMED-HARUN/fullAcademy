using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class AssignmentConfiguration : BaseEntityConfiguration<clsAssignment>
    {
        public override void Configure(EntityTypeBuilder<clsAssignment> builder)
        {
            base.Configure(builder);

            builder.ToTable("Assignments");


            builder.Property(x => x.AssignmentNominationID)
                .IsRequired();

            builder.Property(x => x.CommitteeID)
                .IsRequired();

            builder.Property(x => x.CommitteeType)
                .IsRequired();
            builder.Property(x => x.TotalScore)
        .IsRequired(false);
            builder.Property(x => x.ScheduledDate)
                .IsRequired();

            builder.Property(x => x.AssignmentState)
                .IsRequired();

            builder.HasOne(x => x.AssignmentNomination)
                .WithMany()
                .HasForeignKey(x => x.AssignmentNominationID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
