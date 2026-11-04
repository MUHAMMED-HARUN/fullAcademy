using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Domain.Entities;
using System.Domain.Entities.Address;
using System.Domain.Entities.Attendance;
using System.Domain.Entities.BaseClass;
using System.Domain.Entities.Finance;
using System.Domain.Entities.Institue;
using System.Domain.Entities.Person;
using System.Domain.Entities.Progress;
using System.Infrastructure.Identity;
using System.Infrastructure.Persistence.Seeding;
using System.Reflection;
using System.Reflection.Emit;
namespace System.Infrastructure.Persistence
{
    public class AppDbContext:IdentityDbContext<
        ApplicationUser,
        ApplicationRole,
        Guid>
    {
        private readonly IPublisher _publisher;

        public AppDbContext(DbContextOptions<AppDbContext> options, IPublisher publisher = null) : base(options)
        {
            _publisher = publisher;
        }

       // Address 
        public DbSet<clsAddress> Addresses { get; set; }
        public DbSet<clsCountry> Countries { get; set; }
        public DbSet<clsCity> Cities { get; set; }
        public DbSet<clsDistrict> Districts { get; set; }
        public DbSet<clsNeighborhood> Neighborhoods { get; set; }
        public DbSet<clsPerson> People  { get; set; }
        public DbSet<clsStudent> Students { get; set; }
        public DbSet<clsInstitute> Institutes { get; set; }
        public DbSet<clsProgram> Programs { get; set; }
        public DbSet<clsClass> Classes{ get; set; }
        public DbSet<clsInstituteClass> InstituteClasses{ get; set; }
        public DbSet<clsGroup> Groups { get; set; }
        public DbSet<clsStudentEnrollment> StudentEnrollments { get; set; }
        public DbSet<clsTeachingAssignment> TeachingAssignments{ get; set; }
        public DbSet<clsSession> Sessions{ get; set; }
        public DbSet<clsStage> Stages{ get; set; }
        public DbSet<clsStageInfo> StageInfos{ get; set; }
        public DbSet<clsStudentStage> StudentStages{ get; set; }
        public DbSet<clsDailyEvaluation> DailyEvaluations{ get; set; }
        public DbSet<clsAssignment> Assignments { get; set; }
        public DbSet<clsAssignmentScope> AssignmentScopes { get; set; }
        public DbSet<clsAssignmentScopeinfo> AssignmentScopeInfos { get; set; }
        public DbSet<clsAssignmentNomination> AssignmentNominations { get; set; }
        public DbSet<clsResult> Results { get; set; }

        // Attendance
                public DbSet<clsAttendance> Attendances { get; set; }

        // Finance
        public DbSet<clsFeeStructure> FeeStructures { get; set; }
        public DbSet<clsStudentInvoice> StudentInvoices { get; set; }
        public DbSet<clsPayment> Payments { get; set; }

        // views
        public DbSet<StudentEnrollmentInfoView> vw_StudentEnrollmentInfo { get; set; }
        public DbSet<StudentAvailableNominationScopesView> vw_StudentAvailableNominationScopes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // مهم جداً للـ Identity
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            // تطبيق الـ Data Seeding هنا
            builder.SeedIdentityData();
            
            builder.Entity<StudentEnrollmentInfoView>(entity =>
            {
                entity.ToView("vw_StudentEnrollmentInfo");
                entity.HasNoKey();
            });
            builder.Entity<StudentAvailableNominationScopesView>(entity =>
            {
                entity.ToView("vw_StudentAvailableNominationScopes");
                entity.HasNoKey();
            });
        }
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_publisher != null)
            {
                while (true)
                {
                    var domainEventEntity = ChangeTracker.Entries<clsBaseClass>()
                        .Select(x => x.Entity)
                        .FirstOrDefault(x => x.DomainEvents != null && x.DomainEvents.Any());

                    if (domainEventEntity == null) break;

                    var domainEvent = domainEventEntity.DomainEvents.First();
                    domainEventEntity.RemoveDomainEvent(domainEvent);
                    await _publisher.Publish(domainEvent, cancellationToken);
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
