using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Institute.Program.DTO;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Institute.Institute.Query
{
    class GetProgramByQueryHandler : IRequestHandler<GetProgramByQuery, TResult<ProgramInfo>>
    {
        IProgramRepository _repo;
        public GetProgramByQueryHandler( IProgramRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<ProgramInfo>> Handle(GetProgramByQuery request, CancellationToken cancellationToken)
        {
            ProgramInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<ProgramInfo>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<ProgramInfo>.OK(entityInfo);

        }
    }
}
