
using Gridify;
using System.Application.Services.Enrollment.StudentEnrollment.DTO;
using System.Application.Services.Enrollment.StudentEnrollment.Query;
using System.Application.Services.Institute.Class.DTO;
using System.Application.Services.Institute.Group.DTO;
using System.Application.Services.Institute.Group.Query;
using System.Domain.Entities;
using System.Domain.Enums.Institute;
namespace System.Application.Abstractions.Institute
{
    public interface IGroupRepository
    {
        Task<string> Add(clsGroup entity);// return ID as String
        Task<bool> Update(clsGroup entity);
        Task<List<GroupInfo>> GetList();
        Task<Paging<GroupInfo>> GetListFilter(GetAllGroupQuery request);
        Task<Paging<GroupInfo>> GetMyTeachingGroups(GetMyTeachingGroupsQuery request);

        Task<GroupInfo?> GetBy(string param, enGroupType paramType);
        Task<clsGroup?> GetEntityBy(string param, enGroupType paramType);
        Task<bool> IsExistBy(string param, enGroupType paramType);
        Task<bool> IsAvalibleName(string name,string InstituteClassID);
        Task<bool> IsAvalibleCode(string code,string InstituteClassID);
        Task<bool> IsInstituteClassExist(Guid instituteClassID);
        Task<bool> HasActiveRelationships(Guid groupID);
    }
}
