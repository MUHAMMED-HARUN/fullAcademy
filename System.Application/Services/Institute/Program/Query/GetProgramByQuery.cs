using MediatR;
using System.Application.Abstractions;
using System.Application.Common;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Program.DTO;
using System.Domain.Enums.Institute;

namespace System.Application.Services.Institute.Institute.Query
{
    public class GetProgramByQuery : IRequest<TResult<ProgramInfo>>, IQuery
    {
        public string Param { get; set; }
        public enProgramType ParamType { get; set; }
    }
}
