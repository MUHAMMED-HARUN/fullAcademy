using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Person;
using System.Infrastructure.Configurations.Base;


namespace System.Infrastructure.Configurations.Person
{
    public class StudentConfiguration : BaseEntityConfiguration<clsStudent>
    {
        public override void Configure(EntityTypeBuilder<clsStudent> builder)
        {
            base.Configure(builder);

            builder.ToTable("Students");

            builder.HasOne(x => x.Person)
                   .WithOne() // 1-to-1
                   .HasForeignKey<clsStudent>(x => x.PersonID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
