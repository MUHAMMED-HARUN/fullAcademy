using Gridify;
using Gridify.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Auth;
using System.Application.Abstractions.Enrollment;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Query;
using System.Domain.Entities.Enrollment;
using System.Domain.Enums.AcademicPlan;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Enrollment
{
    public sealed class StudentAcademicEnrollmentService : IStudentAcademicEnrollmentRepository
    {
        private static readonly IGridifyMapper<StudentAcademicEnrollmentLookupInfo> FilterMapper =
            new GridifyMapper<StudentAcademicEnrollmentLookupInfo>()
                .AddMap(nameof(StudentAcademicEnrollmentLookupInfo.StudentName), item => item.StudentName)
                .AddMap(nameof(StudentAcademicEnrollmentLookupInfo.AcademicPlanName), item => item.AcademicPlanName)
                .AddMap(nameof(StudentAcademicEnrollmentLookupInfo.AcademicPlanVersion), item => item.AcademicPlanVersion)
                .AddMap(nameof(StudentAcademicEnrollmentLookupInfo.EnrollmentDate), item => item.EnrollmentDate)
                .AddMap(nameof(StudentAcademicEnrollmentLookupInfo.Status), item => item.Status);

        private readonly AppDbContext _context;
        private readonly DbSet<clsStudentAcademicEnrollment> _set;
        private readonly ICurrentUserService _currentUser;

        public StudentAcademicEnrollmentService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = context.Set<clsStudentAcademicEnrollment>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsStudentAcademicEnrollment entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public Task<bool> IsStudentExist(Guid studentID) =>
            studentID == Guid.Empty
                ? Task.FromResult(false)
                : _context.Students.AsNoTracking().AnyAsync(student => student.Id == studentID);

        public Task<bool> HasActiveEnrollment(Guid studentID, Guid academicPlanID) =>
            _set.AsNoTracking().AnyAsync(enrollment =>
                enrollment.StudentID == studentID &&
                enrollment.AcademicPlanID == academicPlanID &&
                enrollment.Status == enStudentAcademicEnrollmentStatus.Active &&
                (!enrollment.ValidTo.HasValue || enrollment.ValidTo >= DateOnly.FromDateTime(DateTime.Today)));

        public async Task<AcademicPlanEnrollmentSource?> GetActivePlanForEnrollment(Guid academicPlanID)
        {
            return await _context.AcademicPlans.AsNoTracking()
                .Where(plan => plan.Id == academicPlanID && plan.Status == enAcademicPlanStatus.Active)
                .Select(plan => new AcademicPlanEnrollmentSource
                {
                    AcademicPlanID = plan.Id.ToString(),
                    Name = plan.Name,
                    Version = plan.Version,
                    EffectiveFrom = plan.EffectiveFrom,
                    EffectiveTo = plan.EffectiveTo,
                    Items = plan.Items.Select(item => new StudentAcademicEnrollmentItemInfo
                    {
                        StageInfoID = item.StageInfoID.ToString(),
                        From = item.StageInfo.From,
                        To = item.StageInfo.To,
                        StageID = item.StageInfo.StageID.ToString(),
                        StageName = item.StageInfo.Stage.Name,
                        MatterID = item.StageInfo.MatterID.ToString(),
                        MatterName = item.StageInfo.Matter.Name,
                        UniteType = item.StageInfo.UniteType,
                        UniteTypeName = item.StageInfo.UniteType.ToString(),
                        ProgressType = item.StageInfo.ProgressType,
                        ProgressTypeName = item.StageInfo.ProgressType.ToString(),
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<Paging<StudentAcademicEnrollmentLookupInfo>> GetListFilter(GetAllStudentAcademicEnrollmentFilterQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _set.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context);

            if (request.ActiveOnly)
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                query = query.Where(enrollment =>
                    enrollment.Status == enStudentAcademicEnrollmentStatus.Active &&
                    enrollment.ValidFrom <= today &&
                    (!enrollment.ValidTo.HasValue || enrollment.ValidTo >= today));
            }

            if (Guid.TryParse(request.GroupID, out var groupID) && groupID != Guid.Empty)
            {
                var groupClass = await _context.Groups.AsNoTracking()
                    .Where(group => group.Id == groupID)
                    .Select(group => new
                    {
                        group.InstituteClassID,
                        group.InstituteClass.ClassID,
                        group.InstituteClass.Class.ProgramID,
                    })
                    .FirstOrDefaultAsync();
                if (groupClass == null)
                    return new Paging<StudentAcademicEnrollmentLookupInfo>();

                var eligiblePlans = _context.AcademicPlans.AsNoTracking()
                    .Where(plan =>
                        (plan.ScopeType == enAcademicPlanScopeType.Program && plan.ProgramID == groupClass.ProgramID) ||
                        (plan.ScopeType == enAcademicPlanScopeType.InstituteClass && plan.InstituteClassID == groupClass.InstituteClassID) ||
                        (plan.ScopeType == enAcademicPlanScopeType.Stage && _context.Stages.Any(stage => stage.Id == plan.StageID && stage.ClassID == groupClass.ClassID)) ||
                        (plan.ScopeType == enAcademicPlanScopeType.StageInfo && _context.StageInfos.Any(info => info.Id == plan.StageInfoID && _context.Stages.Any(stage => stage.Id == info.StageID && stage.ClassID == groupClass.ClassID))));

                var today = DateOnly.FromDateTime(DateTime.Today);
                query = query.Where(enrollment =>
                    enrollment.Status == enStudentAcademicEnrollmentStatus.Active &&
                    enrollment.ValidFrom <= today && (!enrollment.ValidTo.HasValue || enrollment.ValidTo >= today) &&
                    !_context.StudentEnrollments.Any(membership =>
                        membership.StudentAcademicEnrollmentID == enrollment.Id && membership.GroupID == groupID) &&
                    eligiblePlans.Any(plan => plan.Id == enrollment.AcademicPlanID));
            }

            var projected = query.Select(enrollment => new StudentAcademicEnrollmentLookupInfo
            {
                StudentAcademicEnrollmentID = enrollment.Id.ToString(),
                StudentID = enrollment.StudentID.ToString(),
                StudentName = enrollment.Student.Person.FirstName + " " + enrollment.Student.Person.FatherName + " " + enrollment.Student.Person.LastName,
                AcademicPlanID = enrollment.AcademicPlanID.ToString(),
                AcademicPlanName = enrollment.AcademicPlan.Name,
                AcademicPlanVersion = enrollment.AcademicPlanVersion,
                EnrollmentDate = enrollment.EnrollmentDate,
                Status = enrollment.Status,
                ValidFrom = enrollment.ValidFrom,
                ValidTo = enrollment.ValidTo,
                ItemCount = enrollment.Items.Count,
            });

            return await projected.GridifyAsync(request, FilterMapper);
        }

        public async Task<StudentAcademicEnrollmentInfo?> GetBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context)
                .Where(enrollment => enrollment.Id == id)
                .Select(enrollment => new StudentAcademicEnrollmentInfo
                {
                    StudentAcademicEnrollmentID = enrollment.Id.ToString(),
                    StudentID = enrollment.StudentID.ToString(),
                    StudentName = enrollment.Student.Person.FirstName + " " + enrollment.Student.Person.FatherName + " " + enrollment.Student.Person.LastName,
                    AcademicPlanID = enrollment.AcademicPlanID.ToString(),
                    AcademicPlanName = enrollment.AcademicPlan.Name,
                    AcademicPlanVersion = enrollment.AcademicPlanVersion,
                    EnrollmentDate = enrollment.EnrollmentDate,
                    Status = enrollment.Status,
                    ValidFrom = enrollment.ValidFrom,
                    ValidTo = enrollment.ValidTo,
                    Items = enrollment.Items.Select(item => new StudentAcademicEnrollmentItemInfo
                    {
                        StageInfoID = item.StageInfoID.ToString(),
                        From = item.From,
                        To = item.To,
                        StageID = item.StageID.ToString(),
                        StageName = item.StageName,
                        MatterID = item.MatterID.ToString(),
                        MatterName = item.MatterName,
                        UniteType = item.UniteType,
                        UniteTypeName = item.UniteTypeName,
                        ProgressType = item.ProgressType,
                        ProgressTypeName = item.ProgressTypeName,
                    }).ToList(),
                })
                .FirstOrDefaultAsync();
        }

        public async Task<clsStudentAcademicEnrollment?> GetEntityForInvoice(Guid id)
        {
            return await _set
                .Include(enrollment => enrollment.Student)
                    .ThenInclude(student => student.Person)
                .Include(enrollment => enrollment.AcademicPlan)
                .Include(enrollment => enrollment.Items)
                .AsSplitQuery()
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(enrollment => enrollment.Id == id);
        }
    }
}
