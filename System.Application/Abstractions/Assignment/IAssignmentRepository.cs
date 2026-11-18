using System.Application.Services.Assignment.Assignment.DTO;
using System.Application.Services.Assignment.Assignment.Query;
using Gridify;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IAssignmentRepository
    {
        Task<string> Add(clsAssignment entity);
        Task<bool> Update(clsAssignment entity);
        Task<AssignmentInfo?> GetBy(Guid id);
        Task<AssignmentInfo?> GetForUpsert(Guid id);
        Task<Paging<AssignmentInfo>> GetListFilter(GetAllAssignmentQuery request);
        Task<clsAssignment?> GetEntityBy(Guid id);
        Task<bool> HasAssignment(Guid nominationID);
    }
}
