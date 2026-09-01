
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Person;

namespace System.Application.Abstractions.Institute
{
    public interface ISessionRepository
    {
        Task<string> Add(clsSession entity);// return ID as String
        Task<bool> Update(clsSession entity);
        Task<List<SessionInfo>> GetList();
        Task<SessionInfo> GetBy(string param, enSessionParamType paramType);
        Task<clsSession> GetEntityBy(string param, enSessionParamType paramType);
        Task<bool> IsExistBy(string param, enSessionParamType paramType);
    }
}
