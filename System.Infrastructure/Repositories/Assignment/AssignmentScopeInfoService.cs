using Microsoft.EntityFrameworkCore;
using System.Application.Abstractions.Assignment;
using System.Application.Abstractions.Auth;
using System.Application.Common;
using System.Application.Services.Assignment.AssignmentScopeInfo.DTO;
using System.Domain.Entities;
using System.Infrastructure.Extensions.Security;
using System.Infrastructure.Persistence;

namespace System.Infrastructure.Repositories.Assignment
{
    public class AssignmentScopeInfoService : IAssignmentScopeInfoRepository
    {
        private readonly AppDbContext _context;
        private readonly DbSet<clsAssignmentScopeinfo> _set;
        private readonly ICurrentUserService _currentUser;

        public AssignmentScopeInfoService(AppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _set = _context.Set<clsAssignmentScopeinfo>();
            _currentUser = currentUser;
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
            var query = _set.AsNoTracking()
                .ApplyAssignmentScopeInfoReadScope(_currentUser, _context);

            return await query.Select(x => new AssignmentScopeInfoInfo
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


        public async Task<List<AssignmentScopeInfoInfo>> GetListForAssigmet(Guid assimentID)
        {
            var scopeId = await _context.Assignments
        .AsNoTracking()
        .ApplyAssignmentReadScope(_currentUser, _context)
        .Where(a => a.Id == assimentID)
        .Select(a => a.AssignmentNomination.AssignmentScopeID)
        .FirstOrDefaultAsync();

            return await _set.AsNoTracking()
           .ApplyAssignmentScopeInfoReadScope(_currentUser, _context)
           .Where(x => x.AssignmentScopeID == scopeId &&
               // ?? ����� ����� ����� (!) ���� �������� ���� ��� ��� ����� ����� �� ��� �������� ���
               !_context.Results.Any(r => r.AssignmentID == assimentID &&
                   r.AssignmentScopeInfoID == x.Id && r.IsActive == true)
           )
                .Select(x => new AssignmentScopeInfoInfo
            {
                AssignmentScopeInfoID = x.Id.ToString(),
                MaxScore = x.MaxScore,
                MinPassScore = x.MinPassScore,
                StageInfoMatterNameName = x.StageInfo.Matter.Name,
             
            }).ToListAsync();
          

        }


        public async Task<AssignmentScopeInfoInfo> GetBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentScopeInfoReadScope(_currentUser, _context)
                .Where(x => x.Id == id)
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
            return await _set.ApplyAssignmentScopeInfoReadScope(_currentUser, _context)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> IsExistBy(Guid id)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentScopeInfoReadScope(_currentUser, _context)
                .AnyAsync(x => x.Id == id);
        }

        public async Task<bool> HasAssignmentScopeInfo(Guid StageInfoID, Guid AssignmentScopeID)
        {
            return await _set.AsNoTracking()
                .ApplyAssignmentScopeInfoReadScope(_currentUser, _context)
                .AnyAsync(x => x.StageInfoID == StageInfoID && x.AssignmentScopeID == AssignmentScopeID);
        }

       public  async Task<Dictionary<Guid, clsAssignmentScopeinfo>> GetList(Guid scopeInfoID)
        {
            var scopedSet = _set.AsNoTracking()
                .ApplyAssignmentScopeInfoReadScope(_currentUser, _context);
            Guid assigmentScopeID = await scopedSet.Where(x => x.Id == scopeInfoID)
                .Select(x => x.AssignmentScopeID).FirstOrDefaultAsync();

            var filteredQuery = scopedSet
                .Where(x => x.AssignmentScopeID == assigmentScopeID);

            return await filteredQuery.ToDictionaryAsync(x => x.Id);
        }

    
    }
}
