
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Institute.Query;
using System.Domain.Entities.Institue;
using System.Domain.Enums.Institute;
using Gridify;
namespace System.Application.Abstractions.Institute
{
    public interface IInstituteRepository
    {
        Task<string> Add(clsInstitute entity);// return ID as String
        Task<bool> Update(clsInstitute entity);
        Task<List<InstituteInfo>> GetList(InstituteReadScope readScope);
        Task<Paging<InstituteLookupInfo>> GetListFilter(
            GetInstituteListFilterQuery query,
            InstituteReadScope readScope);
        Task<InstituteInfo> GetBy(string param, enInstituteType paramType, InstituteReadScope readScope);
        Task<clsInstitute> GetEntityBy(string param, enInstituteType paramType);
        Task<bool> IsExistBy(string param, enInstituteType paramType);
        Task<bool> IsExistBy(string param, enInstituteType paramType, InstituteReadScope readScope);
    }
}
