using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Domain.Entities;
using System.Infrastructure.Configurations.Base;

namespace System.Infrastructure.Configurations.Assignment
{
    public class AssignmentScopeInfoConfigurationn : BaseEntityConfiguration<clsAssignmentScopeinfo>
    {
        public override void Configure(EntityTypeBuilder<clsAssignmentScopeinfo> builder)
        {
            base.Configure(builder);

            builder.ToTable("AssignmentScopeInfos");

            builder.Property(x => x.MaxScore)
                .IsRequired();

            builder.Property(x => x.MinPassScore)
                .IsRequired();

            builder.Property(x => x.Weight)
                .IsRequired();

            builder.Property(x => x.StageInfoID)
                .IsRequired();

            builder.Property(x => x.AssignmentScopeID)
                .IsRequired();

            builder.HasOne(x => x.StageInfo)
                .WithMany()
                .HasForeignKey(x => x.StageInfoID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(x => x.AssignmentScope)
                .WithMany()
                .HasForeignKey(x => x.AssignmentScopeID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
