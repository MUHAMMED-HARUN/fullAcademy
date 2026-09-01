using System.Application.Services.Progress.StudentStage.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface IStudentStageRepository
    {
        Task<string> Add(clsStudentStage entity);// return ID as String
        Task<bool> Update(clsStudentStage entity);
        Task<List<StudentStageInfo>> GetList();
        Task<StudentStageInfo> GetBy(string param, enStudentStageParamType paramType);
        Task<clsStudentStage> GetEntityBy(string param, enStudentStageParamType paramType);
        Task<clsStudentStage> GetEntityBy(string studentID, string stageID,enStageStateType state);
        Task<bool> IsExistBy(string param, enStudentStageParamType paramType);
    }
}
