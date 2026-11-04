using System.Application.Services.Finance.FeeStructure.DTO;
using System.Domain.Entities.Finance;
using System.Domain.Enums.Finance;

namespace System.Application.Abstractions.Finance
{
    public interface IFeeStructureRepository
    {
        Task<string> Add(clsFeeStructure entity);
        Task<bool> Update(clsFeeStructure entity);
        Task<List<FeeStructureInfo>> GetList();
        Task<FeeStructureInfo> GetBy(string param, enFeeStructureParamType paramType);
        Task<clsFeeStructure> GetEntityBy(string param, enFeeStructureParamType paramType);
        Task<bool> IsExistBy(string param, enFeeStructureParamType paramType);
    }
}
