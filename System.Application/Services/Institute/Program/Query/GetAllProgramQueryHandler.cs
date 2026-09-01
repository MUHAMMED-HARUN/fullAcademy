using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Common;
using System.Application.Services.Institute.Institute.Query;
using System.Application.Services.Institute.Program.DTO;

namespace System.Application.Services.Person.Person.Query
{
    class GetAllProgramQueryHandler : IRequestHandler<GetAllProgramQuery, TResult<List<ProgramInfo>>>
    {
        IProgramRepository _repo;
        public GetAllProgramQueryHandler ( IProgramRepository repo)
        {
            _repo= repo;
        }
        public async Task<TResult<List<ProgramInfo>>> Handle(GetAllProgramQuery request, CancellationToken cancellationToken)
        {
            return TResult<List<ProgramInfo>>.OK(await _repo.GetList());
        }
    }
}
