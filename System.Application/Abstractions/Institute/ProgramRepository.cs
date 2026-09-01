
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Entities;
using System.Domain.Enums.Institute;

namespace System.Application.Abstractions.Institute
{
    public interface IProgramRepository
    {
        Task<string> Add(clsProgram entity);// return ID as String
        Task<bool> Update(clsProgram entity);
        Task<List<ProgramInfo>> GetList();
        Task<ProgramInfo> GetBy(string param, enProgramType paramType);
        Task<clsProgram> GetEntityBy(string param, enProgramType paramType);
        Task<bool> IsExistBy(string param, enProgramType paramType);
    }
}
