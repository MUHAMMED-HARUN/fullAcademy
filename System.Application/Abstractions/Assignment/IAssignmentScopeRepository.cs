using System.Application.Services.Assignment.AssignmentScope.DTO;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IAssignmentScopeRepository
    {
        Task<string> Add(clsAssignmentScope entity);
        Task<bool> Update(clsAssignmentScope entity);
        Task<List<AssignmentScopeInfo>> GetList();
        Task<List<AssignmentScopeInfo>> GetCanNominate(string studentEnrollmentID);
        Task<AssignmentScopeInfo> GetBy(Guid id);
        Task<clsAssignmentScope> GetEntityBy(Guid id);
        Task<bool> IsExistBy(Guid id);
    }
}
