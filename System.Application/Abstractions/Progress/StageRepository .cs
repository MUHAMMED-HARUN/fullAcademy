
using System.Application.Services.Institute.Program.DTO;
using System.Application.Services.Progress.Stage.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Institute
{
    public interface IStageRepository
    {
        Task<string> Add(clsStage entity);// return ID as String
        Task<bool> Update(clsStage entity);
        Task<List<StageInfo>> GetList();
        Task<StageInfo> GetBy(string param, enStageParamType paramType);
        Task<clsStage> GetEntityBy(string param, enStageParamType paramType);
        Task<bool> IsExistBy(string param, enStageParamType paramType);
    }
}
