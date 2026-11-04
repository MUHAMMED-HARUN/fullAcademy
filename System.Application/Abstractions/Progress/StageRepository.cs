
using Gridify;
using System.Application.Services.Progress.Stage.DTO;
using System.Application.Services.Progress.Stage.Query;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface IStageRepository
    {
        Task<string> Add(clsStage entity);// return ID as String
        Task<bool> Update(clsStage entity);
        Task<List<StageInfo>> GetList();
        Task<Paging<StageLookupInfo>> GetListFilter(GetAllStageFilterQuery request);
        Task<StageInfo?> GetBy(string param, enStageParamType paramType);
        Task<clsStage?> GetEntityBy(string param, enStageParamType paramType);
        Task<bool> IsExistBy(string param, enStageParamType paramType);
        Task<bool> IsNameExistInClass(string name, Guid classID, Guid? excludedStageID = null);
        Task<bool> IsValidPreviousStage(Guid classID, Guid? previousStageID, Guid? currentStageID = null);
    }
}
