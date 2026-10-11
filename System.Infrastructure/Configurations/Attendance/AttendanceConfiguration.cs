using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Attendance;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class AttendanceConfiguration : BaseEntityConfiguration<clsAttendance>
    {
        public override void Configure(EntityTypeBuilder<clsAttendance> builder)
        {
            base.Configure(builder);

            builder.ToTable("Attendances");


            builder.Property(x => x.AttendeeType)
                .IsRequired();

            builder.Property(x => x.AttendeeID)
                .IsRequired();

            builder.Property(x => x.Date)
                .IsRequired();
            builder.Property(x => x.CheckInTime)
        .IsRequired(false);
            builder.Property(x => x.CheckOutTime)
        .IsRequired(false);
            builder.Property(x => x.Status)
                .IsRequired();

            builder.Property(x => x.ContextID)
                .IsRequired(false);

            builder.Property(x => x.LeaveRequestID)
                .IsRequired(false);
            builder.Property(x => x.Notes).HasMaxLength(200)
                .IsRequired(false);

            builder.HasOne(x => x.LeaveRequest)
                .WithMany()
                .HasForeignKey(x => x.LeaveRequestID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
