using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Institue;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Institute.Institute
{
    public class ClassConfiguration : BaseEntityConfiguration<clsClass>
    {
        public override void Configure(EntityTypeBuilder<clsClass> builder)
        {
            base.Configure(builder);

            builder.ToTable("Classes");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Note).IsRequired(false).HasMaxLength(500);
            builder.Property(x => x.Level).IsRequired();
            builder.Property(x => x.ProgramID).IsRequired();

            builder.HasOne(x => x.Program)
                   .WithMany()
                   .HasForeignKey(x => x.ProgramID)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
