using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Assignment.Assignment.DTO;
using System.Application.Services.Assignment.Assignment.Query;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentService : IAssignmentRepository
    {
        private static readonly IGridifyMapper<AssignmentInfo> AssignmentFilterMapper =
            new GridifyMapper<AssignmentInfo>()
                .AddMap(nameof(AssignmentInfo.StudentName), info => info.StudentName)
                .AddMap(nameof(AssignmentInfo.AcademicPlanName), info => info.AcademicPlanName)
                .AddMap(nameof(AssignmentInfo.AssigmentScopeName), info => info.AssigmentScopeName)
                .AddMap(nameof(AssignmentInfo.AssignmentState), info => info.AssignmentState)
                .AddMap(nameof(AssignmentInfo.ScheduledDate), info => info.ScheduledDate);

        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignment> _set;
        private readonly ICurrentUserService _currentUser;

        public AssignmentService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsAssignment>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsAssignment entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsAssignment entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<Paging<AssignmentInfo>> GetListFilter(GetAllAssignmentQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);
            var query = _set.AsNoTracking()
                .ApplyAssignmentReadScope(_currentUser, _context);
            return await ProjectAssignment(query).GridifyAsync(request, AssignmentFilterMapper);
        }

        public async Task<AssignmentInfo?> GetBy(Guid id)
        {
            var query = _set.AsNoTracking()
                .ApplyAssignmentReadScope(_currentUser, _context)
                .Where(assignment => assignment.Id == id);
            return await ProjectAssignment(query).FirstOrDefaultAsync();
        }

        public Task<AssignmentInfo?> GetForUpsert(Guid id) => GetBy(id);

        public async Task<clsAssignment?> GetEntityBy(Guid id)
        {
            return await _set.ApplyAssignmentReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> HasAssignment(Guid nominationID)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentReadScope(_currentUser, _context)
                .AnyAsync(x => x.AssignmentNominationID == nominationID &&
                    (x.AssignmentState == enAssignmentType.inProgress ||
                     x.AssignmentState == enAssignmentType.Completed) &&
                    x.IsActive && x.DeletedAt == null);
        }

        private IQueryable<AssignmentInfo> ProjectAssignment(IQueryable<clsAssignment> query)
        {
            return query.Select(assignment => new AssignmentInfo
            {
                AssignmentID = assignment.Id.ToString(),
                AssignmentNominationID = assignment.AssignmentNominationID.ToString(),
                StudentAcademicEnrollmentID = assignment.AssignmentNomination.StudentAcademicEnrollmentID.ToString(),
                AcademicPlanName = assignment.AssignmentNomination.StudentAcademicEnrollment.AcademicPlan.Name,
                ScheduledDate = assignment.ScheduledDate.ToString(),
                AssignmentState = assignment.AssignmentState.ToString(),
                IsPassed = assignment.IsPassed,
                AssigmentScopeName = assignment.AssignmentNomination.AssignmentScope.Name,
                StudentName = assignment.AssignmentNomination.StudentAcademicEnrollment.Student.Person.FirstName + " " +
                    assignment.AssignmentNomination.StudentAcademicEnrollment.Student.Person.FatherName + " " +
                    assignment.AssignmentNomination.StudentAcademicEnrollment.Student.Person.LastName,
                AssignmentScopeID = assignment.AssignmentNomination.AssignmentScopeID.ToString(),
                CommitteeID = assignment.CommitteeID.ToString(),
                CommitteeType = assignment.CommitteeType,
                CanAddResult = !_context.Results.Any(result => result.AssignmentID == assignment.Id && result.IsActive) ||
                    (assignment.IsActive &&
                     _context.Results.Count(result => result.AssignmentID == assignment.Id && result.IsActive) <
                     _context.AssignmentScopeInfos.Count(info =>
                         info.AssignmentScopeID == assignment.AssignmentNomination.AssignmentScopeID && info.IsActive)),
                TotalScore = assignment.TotalScore,
            });
        }
    }
}
