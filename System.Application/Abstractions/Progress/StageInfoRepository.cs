
using Gridify;
using System.Application.Services.Progress.StageInfo.DTO;
using System.Application.Services.Progress.StageInfo.Query;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface IStageInfoRepository
    {
        Task<string> Add(clsStageInfo entity);// return ID as String
        Task<bool> Update(clsStageInfo entity);
        Task<List<StageInfoInfo>> GetList();
        Task<Paging<StageInfoInfo>> GetListFilter(GetAllStageInfoFilterQuery request);
        Task<List<StageInfoInfo>> GetStudentStageInfo(string studentAcademicEnrollmentID);
        Task<StageInfoInfo?> GetBy(string param, enStageInfoParamType paramType);
        Task<clsStageInfo?> GetEntityBy(string param, enStageInfoParamType paramType);
        Task<bool> IsExistBy(string param, enStageInfoParamType paramType);
    }
}
