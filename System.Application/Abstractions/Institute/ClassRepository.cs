
using System.Application.Services.Institute.Class.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Institute;
namespace System.Application.Abstractions.Institute
{
    public interface IClassRepository
    {
        Task<string> Add(clsClass entity);// return ID as String
        Task<bool> Update(clsClass entity);
        Task<List<ClassInfo>> GetList();
        Task<ClassInfo> GetBy(string param, enClassType paramType);
        Task<clsClass> GetEntityBy(string param, enClassType paramType);
        Task<bool> IsExistBy(string param, enClassType paramType);
        Task<bool> IsAvalibleName(string name,int? Level=null,string programID=null);
        Task<bool> IsAvalibleLevel(int Level,string programID);
    }
}
