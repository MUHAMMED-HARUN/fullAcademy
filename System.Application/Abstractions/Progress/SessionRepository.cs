
using Gridify;
using System.Application.Services.Progress.Session.DTO;
using System.Application.Services.Progress.Session.Query;
using System.Domain.Entities.Progress;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Progress
{
    public interface ISessionRepository
    {
        Task<string> Add(clsSession entity);// return ID as String
        Task<bool> Update(clsSession entity);
        Task<List<SessionInfo>> GetList();
        Task<Paging<SessionLookupInfo>> GetListFilter(GetAllSessionFilterQuery request);
        Task<SessionInfo?> GetBy(string param, enSessionParamType paramType);
        Task<clsSession?> GetEntityBy(string param, enSessionParamType paramType);
        Task<bool> IsExistBy(string param, enSessionParamType paramType);
        Task<bool> IsExistNameByType(string name, enSessionType sessionType, Guid? excludedSessionID = null);
        Task<bool> HasChildren(Guid sessionID);
    }
}
