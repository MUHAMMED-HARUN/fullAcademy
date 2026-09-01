using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.Program.DTO;
namespace System.Application.Services.Institute.Institute.Query{
    public class GetAllProgramQuery : IRequest<TResult<List<ProgramInfo>>>, IQuery
    {
    }
}
