
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Abstractions.Institute
{
    public interface IInstituteClassRepository
    {
        Task<string> Add(clsInstituteClass entity);// return ID as String
        Task<bool> Update(clsInstituteClass entity);
        Task<List<InstituteClassInfo>> GetList();
        Task<InstituteClassInfo> GetBy(string param, enInstituteClassType paramType);
        Task<clsInstituteClass> GetEntityBy(string param, enInstituteClassType paramType);
        Task<bool> IsExistBy(string param, enInstituteClassType paramType);
        Task<bool> IsClassInInstituteBy(string ClassID, string InstituteID);
    }
}
