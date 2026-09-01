using System.Application.Services.Assignment.Assignment.DTO;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IAssignmentRepository
    {
        Task<string> Add(clsAssignment entity);
        Task<bool> Update(clsAssignment entity);
        Task<List<AssignmentInfo>> GetList();
        Task<AssignmentInfo> GetBy(Guid id);
        Task<clsAssignment> GetEntityBy(Guid id);
        Task<bool> IsExistBy(Guid id);
        Task<bool> HasAssignment(Guid NominationID);
    }
}
