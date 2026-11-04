

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Progress;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Progress
{
    public class SessionConfiguration : BaseEntityConfiguration<clsSession>
    {
        public override void Configure(EntityTypeBuilder<clsSession> builder)
        {
            base.Configure(builder);

            builder.ToTable("Subjects");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Notes).HasMaxLength(500);
            builder.HasOne(x => x.Session)
                 .WithMany(x => x.ChildSessions)
                 .HasForeignKey(x => x.SessionID)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
