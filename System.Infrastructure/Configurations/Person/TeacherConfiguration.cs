using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Person;
using System.Infrastructure.Configurations.Base;


namespace System.Infrastructure.Configurations.Person
{
    public class TeacherConfiguration : BaseEntityConfiguration<clsTeacher>
    {
        public override void Configure(EntityTypeBuilder<clsTeacher> builder)
        {
            base.Configure(builder);

            builder.ToTable("Teachers");

            builder.HasOne(x => x.Person)
                   .WithOne() // 1-to-1
                   .HasForeignKey<clsTeacher>(x => x.PersonID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
