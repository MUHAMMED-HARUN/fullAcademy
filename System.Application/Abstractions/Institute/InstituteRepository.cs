
using System.Application.Services.Institute.DTO;
using System.Domain.Entities.Institue;
using System.Domain.Enums.Institute;
namespace System.Application.Abstractions.Institute
{
    public interface IInstituteRepository
    {
        Task<string> Add(clsInstitute entity);// return ID as String
        Task<bool> Update(clsInstitute entity);
        Task<List<InstituteInfo>> GetList();
        Task<InstituteInfo> GetBy(string param, enInstituteType paramType);
        Task<clsInstitute> GetEntityBy(string param, enInstituteType paramType);
        Task<bool> IsExistBy(string param, enInstituteType paramType);
    }
}
