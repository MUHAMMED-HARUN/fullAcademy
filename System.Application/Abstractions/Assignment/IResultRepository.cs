using System.Application.Services.Assignment.Result.DTO;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IResultRepository
    {
        Task<string> Add(clsResult entity);
        Task<bool> Update(clsResult entity);
        Task<List<ResultInfo>> GetList();
        Task<List<clsResult>> GetList(Guid[] scopeInfos );
        Task<List<clsResult>> GetListByAssignmentID(Guid AssignmentID);
        Task<ResultInfo> GetBy(Guid id);
        Task<clsResult> GetEntityBy(Guid id);
        Task<bool> IsExistBy(Guid id);
        Task<bool> HasResult(Guid AssigmentID, Guid AssigmentScopeInfoID);
    }
}
