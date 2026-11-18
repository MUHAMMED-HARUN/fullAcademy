using System.Application.Services.Assignment.Result.DTO;
using System.Application.Services.Assignment.Result.Query;
using Gridify;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IResultRepository
    {
        Task<string> Add(clsResult entity);
        Task<bool> Update(clsResult entity);
        Task<Paging<ResultInfo>> GetListFilter(GetAllResultQuery request);
        Task<ResultInfo?> GetBy(Guid id);
        Task<ResultInfo?> GetForUpsert(Guid id);
        Task<clsResult?> GetEntityBy(Guid id);
        Task<List<clsResult>> GetListByAssignmentID(Guid assignmentID);
        Task<bool> HasResult(Guid assignmentID, Guid assignmentScopeInfoID);
    }
}
