using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScopeInfo.DTO;
using System.Domain.Entities;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentScopeInfoService : IAssignmentScopeInfoRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignmentScopeinfo> _set;

        public AssignmentScopeInfoService(AppDbContext context)
        {
            _context = context;
            _set = _context.Set<clsAssignmentScopeinfo>();
        }

        public async Task<string> Add(clsAssignmentScopeinfo entity)
        {
            await _set.AddAsync(entity);
            return entity.Id.ToString();
        }

        public async Task<bool> Update(clsAssignmentScopeinfo entity)
        {
            _set.Update(entity);
            return await Task.FromResult(true);
        }

        public async Task<List<AssignmentScopeInfoInfo>> GetList()
        {
            return await _set.Select(x => new AssignmentScopeInfoInfo
            {
                AssignmentScopeInfoID = x.Id.ToString(),
                MaxScore = x.MaxScore,
                MinPassScore = x.MinPassScore,
                Weight = x.Weight,
                StageInfoID = x.StageInfoID.ToString(),
                AssignmentScopeID = x.AssignmentScopeID.ToString(),
                AssignmentScopeName=x.AssignmentScope.Name,
                StageInfoMatterNameName=x.StageInfo.Matter.Name
            }).ToListAsync();
        }

        public async Task<AssignmentScopeInfoInfo> GetBy(Guid id)
        {
            return await _set.Where(x => x.Id == id)
                .Select(x => new AssignmentScopeInfoInfo
                {
                    AssignmentScopeInfoID = x.Id.ToString(),
                    MaxScore = x.MaxScore,
                    MinPassScore = x.MinPassScore,
                    Weight = x.Weight,
                    StageInfoID = x.StageInfoID.ToString(),
                    AssignmentScopeID = x.AssignmentScopeID.ToString(),
                    AssignmentScopeName = x.AssignmentScope.Name,
                    StageInfoMatterNameName = x.StageInfo.Matter.Name
                })
                .FirstOrDefaultAsync();
        }

        public async Task<clsAssignmentScopeinfo> GetEntityBy(Guid id)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasAssignmentScopeInfo(Guid StageInfoID, Guid AssignmentScopeID)
        {
            return await _set.AnyAsync(x => x.StageInfoID == StageInfoID && x.AssignmentScopeID == AssignmentScopeID);
        }

      public  async Task<Dictionary<Guid, clsAssignmentScopeinfo>> GetList(Guid scopeInfoID)
        {
            Guid assigmentScopeID =await _set.Where(x => x.Id == scopeInfoID).Select(x => x.AssignmentScopeID).FirstOrDefaultAsync();

            var filteredQuery = _set
                   .Where(x => x.AssignmentScopeID == assigmentScopeID);

            return await filteredQuery.ToDictionaryAsync(x => x.Id);
        }

    
    }
}
