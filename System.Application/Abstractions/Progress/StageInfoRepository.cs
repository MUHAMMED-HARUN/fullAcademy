
using System.Application.Services.Progress.StageInfo.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Institute
{
    public interface IStageInfoRepository
    {
        Task<string> Add(clsStageInfo entity);// return ID as String
        Task<bool> Update(clsStageInfo entity);
        Task<List<StageInfoInfo>> GetList();
        Task<List<StageInfoInfo>> GetStudentStageInfo(string StudentEnrollmentID);
        Task<StageInfoInfo> GetBy(string param, enStageInfoParamType paramType);
        Task<clsStageInfo> GetEntityBy(string param, enStageInfoParamType paramType);
        Task<bool> IsExistBy(string param, enStageInfoParamType paramType);
    }
}
