using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities;
using System.Domain.Entities.Institue;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Institute.Institute
{
    public class ProgramConfiguration : BaseEntityConfiguration<clsProgram>
    {
        public override void Configure(EntityTypeBuilder<clsProgram> builder)
        {
            base.Configure(builder);

            builder.ToTable("Programs");

            builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Notes).HasMaxLength(500);

           
        }
    }
}
