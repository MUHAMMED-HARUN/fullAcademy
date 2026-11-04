

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.Person;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Person
{
    public class PersonConfiguration : BaseEntityConfiguration<clsPerson>
    {
        public override void Configure(EntityTypeBuilder<clsPerson> builder)
        {
            base.Configure(builder);

            builder.ToTable("People");

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(60);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(60);
            builder.Property(x => x.FatherName).IsRequired().HasMaxLength(60);
            builder.Property(x => x.MotherName).IsRequired().HasMaxLength(60);
            builder.Property(x => x.MotherLastName).IsRequired().HasMaxLength(60);
            builder.HasIndex(x => x.NationalNumber).IsUnique();
            builder.Property(x => x.Gendor).IsRequired();
            builder.Property(x => x.BirthDate).IsRequired();
            builder.HasOne(x => x.PlaceOfBirth)
                  .WithMany()
                  .HasForeignKey(x => x.PlaceOfBirthID)
                  .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.LivingPlace)
                   .WithMany()
                   .HasForeignKey(x => x.LivingPlaceID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
