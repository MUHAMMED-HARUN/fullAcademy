using Microsoft.EntityFrameworkCore;
using Gridify;
using Gridify.EntityFramework;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentNomination.DTO;
using System.Application.Services.Assignment.AssignmentNomination.Query;
using System.Domain.Entities;
using System.Domain.Enums.Enrollment;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentNominationService : IAssignmentNominationRepository
    {
        private static readonly IGridifyMapper<AssignmentNominationInfo> FilterMapper =
            new GridifyMapper<AssignmentNominationInfo>()
                .AddMap(nameof(AssignmentNominationInfo.StudentName), item => item.StudentName)
                .AddMap(nameof(AssignmentNominationInfo.AcademicPlanName), item => item.AcademicPlanName)
                .AddMap(nameof(AssignmentNominationInfo.AssignmentScopeName), item => item.AssignmentScopeName)
                .AddMap(nameof(AssignmentNominationInfo.NominationState), item => item.NominationState)
                .AddMap(nameof(AssignmentNominationInfo.Date), item => item.Date);

        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignmentNomination> _set;
        private readonly ICurrentUserService _currentUser;

        public AssignmentNominationService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsAssignmentNomination>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsAssignmentNomination entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsAssignmentNomination entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<List<AssignmentNominationInfo>> GetList()
        {
            var query = _set.AsNoTracking()
                .ApplyAssignmentNominationReadScope(_currentUser, _context);

            return await Project(query).ToListAsync();
        }

        public async Task<Paging<AssignmentNominationInfo>> GetListFilter(GetAllAssignmentNominationQuery request)
        {
            request.Page = Math.Max(1, request.Page);
            request.PageSize = Math.Clamp(request.PageSize, 1, 100);
            var query = _set.AsNoTracking()
                .ApplyAssignmentNominationReadScope(_currentUser, _context);
            return await Project(query).GridifyAsync(request, FilterMapper);
        }

        public async Task<AssignmentNominationInfo?> GetBy(Guid id)
        {
            var query = _set.AsNoTracking()
                .ApplyAssignmentNominationReadScope(_currentUser, _context)
                .Where(x => x.Id == id);
            return await Project(query).FirstOrDefaultAsync();
        }

        public async Task<clsAssignmentNomination?> GetEntityBy(Guid id)
        {
            return await _set.ApplyAssignmentNominationReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentNominationReadScope(_currentUser, _context)
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasNomination(Guid studentAcademicEnrollmentID, Guid assignmentScopeID)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentNominationReadScope(_currentUser, _context)
                .AnyAsync(x => x.StudentAcademicEnrollmentID == studentAcademicEnrollmentID &&
                    x.AssignmentScopeID == assignmentScopeID &&
                    (x.NominationState == enAssignmentType.Completed ||
                     x.NominationState == enAssignmentType.inProgress) &&
                    x.IsActive && x.DeletedAt == null);
        }

        public async Task<bool> IsScopeIncludedInAcademicEnrollment(Guid studentAcademicEnrollmentID, Guid assignmentScopeID)
        {
            var allowedEnrollments = _context.StudentAcademicEnrollments.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context);
            var allowedScopes = _context.AssignmentScopes.AsNoTracking()
                .ApplyAssignmentScopeReadScope(_currentUser, _context);

            return await allowedEnrollments.AnyAsync(enrollment =>
                enrollment.Id == studentAcademicEnrollmentID &&
                allowedScopes.Any(scope => scope.Id == assignmentScopeID) &&
                _context.AssignmentScopeInfos.AsNoTracking()
                    .Any(info => info.AssignmentScopeID == assignmentScopeID) &&
                !_context.AssignmentScopeInfos.AsNoTracking()
                    .Any(info => info.AssignmentScopeID == assignmentScopeID &&
                        !enrollment.Items.Any(item => item.StageInfoID == info.StageInfoID)));
        }

        public async Task<clsAssignmentNomination?> GetEntityByAssigmetID(Guid AssigmetID)
        {
            var query = _set.ApplyAssignmentNominationReadScope(_currentUser, _context)
                .Join(_context.Assignments,
                 assigNomini => assigNomini.Id,
                 assig => assig.AssignmentNominationID,
                 (assigNomini, assig) => new { assigNomini, assig });

          return await  query.Where(x => x.assig.Id == AssigmetID).Select(x=>x.assigNomini).FirstOrDefaultAsync();
        }

        private IQueryable<AssignmentNominationInfo> Project(IQueryable<clsAssignmentNomination> query)
        {
            return query.Select(x => new AssignmentNominationInfo
            {
                AssignmentNominationID = x.Id.ToString(),
                StudentAcademicEnrollmentID = x.StudentAcademicEnrollmentID.ToString(),
                AssignmentScopeID = x.AssignmentScopeID.ToString(),
                Date = x.Date.ToString(),
                AssignmentScopeName = x.AssignmentScope.Name,
                StudentName = x.StudentAcademicEnrollment.Student.Person.FirstName + " " +
                    x.StudentAcademicEnrollment.Student.Person.FatherName + " " +
                    x.StudentAcademicEnrollment.Student.Person.LastName,
                AcademicPlanName = x.StudentAcademicEnrollment.AcademicPlan.Name,
                NominationState = x.NominationState.ToString(),
                CanStartAssigment = x.NominationState == enAssignmentType.inProgress &&
                    !_context.Assignments.Any(assignment =>
                        assignment.AssignmentNominationID == x.Id &&
                        assignment.IsActive && assignment.AssignmentState != enAssignmentType.Canceled),
            });
        }
    }
}
