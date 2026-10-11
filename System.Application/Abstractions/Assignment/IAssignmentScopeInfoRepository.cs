using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Application.Services.Assignment.AssignmentScopeInfo.DTO;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IAssignmentScopeInfoRepository
    {
        Task<string> Add(clsAssignmentScopeinfo entity);
        Task<bool> Update(clsAssignmentScopeinfo entity);
        Task<List<AssignmentScopeInfoInfo>> GetList();
        Task<Dictionary<Guid,clsAssignmentScopeinfo>> GetList(Guid scopeInfoID);
        Task<AssignmentScopeInfoInfo> GetBy(Guid id);
        Task<List<AssignmentScopeInfoInfo>> GetListForAssigmet(Guid assimentID);
        Task<clsAssignmentScopeinfo> GetEntityBy(Guid id);
        Task<bool> IsExistBy(Guid id);
        Task<bool> HasAssignmentScopeInfo(Guid StageInfoID, Guid AssignmentScopeID);
    }
}
