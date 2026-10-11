using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class LeaveRequestConfiguration : BaseEntityConfiguration<clsLeaveRequest>
    {
        public override void Configure(EntityTypeBuilder<clsLeaveRequest> builder)
        {
            base.Configure(builder);

            builder.ToTable("LeaveRequests");


            builder.Property(x => x.AttendeeType)
                .IsRequired();

            builder.Property(x => x.EntityID)
                .IsRequired();

            builder.Property(x => x.LeaveType)
                .IsRequired();

            builder.Property(x => x.StartDate)
        .IsRequired();
            builder.Property(x => x.EndDate)
        .IsRequired();

            builder.Property(x => x.Reason).HasMaxLength(200)
                .IsRequired(false);

            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.ApprovedByID)
                .IsRequired(false);
            builder.Property(x => x.RejectionReason).HasMaxLength(200)
                .IsRequired(false);
        }
    }
}
