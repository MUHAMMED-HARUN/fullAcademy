using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Domain.Entities;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentScopeService : IAssignmentScopeRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignmentScope> _set;
        private readonly ICurrentUserService _currentUser;

        public AssignmentScopeService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsAssignmentScope>();
            _currentUser = currentUser;
        }

        public async Task<string> Add(clsAssignmentScope entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsAssignmentScope entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<List<AssignmentScopeInfo>> GetList()
        {
            var query = _set.AsNoTracking()
                .ApplyAssignmentScopeReadScope(_currentUser, _context);

            return await query.Select(x => new AssignmentScopeInfo
            {
                AssignmentScopeID = x.Id.ToString(),
                Name = x.Name,
                Notes = x.Notes,
                StageID = x.StageID.ToString(),
                StageName = x.Stage.Name,
                PrevAssignmentScopeID = x.PrevAssignmentScopeID.ToString(),
                PrevAssignmentScopeName = x.PrevAssignmentScope.Name
            }).ToListAsync();
        }

        public async Task<AssignmentScopeInfo> GetBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentScopeReadScope(_currentUser, _context)
                .Where(x => x.Id == id)
                .Select(x => new AssignmentScopeInfo
                {
                    AssignmentScopeID = x.Id.ToString(),
                    Name = x.Name,
                    Notes = x.Notes,
                    StageID = x.StageID.ToString(),
                    StageName = x.Stage.Name,
                    PrevAssignmentScopeID = x.PrevAssignmentScopeID.ToString(),
                    PrevAssignmentScopeName = x.PrevAssignmentScope.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<clsAssignmentScope> GetEntityBy(Guid id)
        {
            return await _set.ApplyAssignmentScopeReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentScopeReadScope(_currentUser, _context)
                .AnyAsync(x => x.Id == id);
        }

        public async Task<List<AssignmentScopeInfo>> GetCanNominate(string studentAcademicEnrollmentID)
        {
            if (!Guid.TryParse(studentAcademicEnrollmentID, out var enrollmentID) || enrollmentID == Guid.Empty)
                return new List<AssignmentScopeInfo>();

            var today = DateOnly.FromDateTime(DateTime.Today);
            var academicEnrollmentIsInScope = await _context.StudentAcademicEnrollments.AsNoTracking()
                .ApplyStudentAcademicEnrollmentReadScope(_currentUser, _context)
                .AnyAsync(enrollment => enrollment.Id == enrollmentID &&
                    enrollment.Status == System.Domain.Enums.Enrollment.enStudentAcademicEnrollmentStatus.Active &&
                    enrollment.ValidFrom <= today && (!enrollment.ValidTo.HasValue || enrollment.ValidTo >= today));
            if (!academicEnrollmentIsInScope)
                return new List<AssignmentScopeInfo>();

            var allowedScopeIDs = _set.AsNoTracking()
                .ApplyAssignmentScopeReadScope(_currentUser, _context)
                .Select(scope => scope.Id);

            return await _context.vw_StudentAvailableNominationScopes.AsNoTracking()
                .Where(item => item.StudentAcademicEnrollmentID == enrollmentID &&
                    allowedScopeIDs.Contains(item.AssignmentScopeID))
                .Select(item => new AssignmentScopeInfo
                {
                    AssignmentScopeID = item.AssignmentScopeID.ToString(),
                    Name = item.AssignmentScopeName,
                }).ToListAsync();

        }
    }
}
