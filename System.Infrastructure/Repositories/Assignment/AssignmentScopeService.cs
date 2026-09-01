using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentScopeService : IAssignmentScopeRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignmentScope> _set;

        public AssignmentScopeService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsAssignmentScope>();
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
            return await _set.Select(x => new AssignmentScopeInfo
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
            return await _set.Where(x => x.Id == id)
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
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AnyAsync(x => x.Id == id);
        }

        public async Task<List<AssignmentScopeInfo>> GetCanNominate(string studentEnrollmentID)
        {
           return await _context.vw_StudentAvailableNominationScopes.Where(x=>x.EnrollmentID==clsUtil.ToGuid( studentEnrollmentID)).Select(x => new AssignmentScopeInfo
            {
                AssignmentScopeID = x.AssignmentScopeID.ToString(),
                Name = x.AssignmentScopeName.ToString(),

            }).ToListAsync();

        }
    }
}
