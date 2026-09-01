using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Domain.Entities.BaseClass;

namespace System.Infrastructure.Configurations.Base
{
    public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : clsBaseClass
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(bc => bc.Id);
            builder.Property(bc => bc.Id).ValueGeneratedNever();

            builder.Property(bc => bc.CreatedBy).IsRequired(false);
            builder.Property(bc => bc.ModifiedBy).IsRequired(false);
            builder.Property(bc => bc.DeletedBy).IsRequired(false);

            builder.Property(bc => bc.CreatedAt).IsRequired(true); // Assuming CreatedAt is required as per logic
            builder.Property(bc => bc.ModifiedAt).IsRequired(false);
            builder.Property(bc => bc.DeletedAt).IsRequired(false);
            builder.Property(bc => bc.IsActive).IsRequired(true); // Usually true/false

            // Soft Delete Filter as per user request (DeletedBy != null)
            builder.HasQueryFilter(bc => bc.DeletedAt == null);
        }
    }
}
