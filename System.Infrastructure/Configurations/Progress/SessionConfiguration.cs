

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Person;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Address
{

    public class SubjectConfiguration : BaseEntityConfiguration<clsSession>
    {
        public override void Configure(EntityTypeBuilder<clsSession> builder)
        {
            base.Configure(builder);

            builder.ToTable("Subjects");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Notes).HasMaxLength(500);
            builder.HasOne(x => x.Session)
                 .WithOne() // 1-to-1
                 .HasForeignKey<clsSession>(x => x.SessionID)
                 .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
