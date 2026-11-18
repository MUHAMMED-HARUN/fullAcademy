using System.Application.Services.Progress.StudentStage.DTO;
using System.Application.Services.Progress.StudentStage.Query;
using Gridify;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface IStudentStageRepository
    {
        Task<string> Add(clsStudentStage entity);// return ID as String
        Task<bool> Update(clsStudentStage entity);
        Task<Paging<StudentStageInfo>> GetListFilter(GetAllStudentStageQuery request);
        Task<StudentStageInfo?> GetBy(string param, enStudentStageParamType paramType);
        Task<clsStudentStage?> GetEntityBy(string param, enStudentStageParamType paramType);
        Task<clsStudentStage?> GetEntityBy(string studentAcademicEnrollmentID, string stageID, enStageStateType state);
        Task<Guid?> GetPreviousStageID(Guid stageID);
        Task<bool> IsExistBy(string param, enStudentStageParamType paramType);
    }
}
