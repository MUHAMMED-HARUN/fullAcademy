using Gridify;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.DTO;
using System.Application.Services.Enrollment.StudentAcademicEnrollment.Query;
using System.Domain.Entities.Enrollment;

namespace System.Application.Abstractions.Enrollment
{
    public interface IStudentAcademicEnrollmentRepository
    {
        Task<string> Add(clsStudentAcademicEnrollment entity);
        Task<bool> IsStudentExist(Guid studentID);
        Task<bool> HasActiveEnrollment(Guid studentID, Guid academicPlanID);
        Task<AcademicPlanEnrollmentSource?> GetActivePlanForEnrollment(Guid academicPlanID);
        Task<Paging<StudentAcademicEnrollmentLookupInfo>> GetListFilter(GetAllStudentAcademicEnrollmentFilterQuery request);
        Task<StudentAcademicEnrollmentInfo?> GetBy(Guid id);
        Task<clsStudentAcademicEnrollment?> GetEntityForInvoice(Guid id);
    }
}
