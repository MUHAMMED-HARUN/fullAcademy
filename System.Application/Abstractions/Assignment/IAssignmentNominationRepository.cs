using System.Application.Services.Assignment.AssignmentNomination.DTO;
using System.Application.Services.Assignment.AssignmentNomination.Query;
using System.Application.Common;
using Gridify;
using System.Domain.Entities;

namespace System.Application.Abstractions.Assignment
{
    public interface IAssignmentNominationRepository
    {
        Task<string> Add(clsAssignmentNomination entity);
        Task<bool> Update(clsAssignmentNomination entity);
        Task<List<AssignmentNominationInfo>> GetList();
        Task<Paging<AssignmentNominationInfo>> GetListFilter(GetAllAssignmentNominationQuery request);
        Task<AssignmentNominationInfo?> GetBy(Guid id);
        Task<clsAssignmentNomination?> GetEntityBy(Guid id);
        Task<clsAssignmentNomination?> GetEntityByAssigmetID(Guid AssigmetID);
        Task<bool> IsExistBy(Guid id);
        Task<bool> HasNomination(Guid studentAcademicEnrollmentID, Guid assignmentScopeID);
        Task<bool> IsScopeIncludedInAcademicEnrollment(Guid studentAcademicEnrollmentID, Guid assignmentScopeID);
    }
}
