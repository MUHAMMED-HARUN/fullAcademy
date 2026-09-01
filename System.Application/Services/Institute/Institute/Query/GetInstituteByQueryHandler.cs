using MediatR;
using System.Application.Abstractions.Institute;
using System.Application.Abstractions.Person;
using System.Application.Common;
using System.Application.Exceptions;
using System.Application.Services.Institute.DTO;
using System.Application.Services.Person.Person.DTO;

namespace System.Application.Services.Institute.Institute.Query
{
    class GetInstituteByQueryHandler : IRequestHandler<GetInstituteByQuery, TResult<InstituteInfo>>
    {
        IInstituteRepository _repo;
        public GetInstituteByQueryHandler( IInstituteRepository repo)
        {
            _repo = repo;
        }
        public async Task<TResult<InstituteInfo>> Handle(GetInstituteByQuery request, CancellationToken cancellationToken)
        {
            InstituteInfo entityInfo = await _repo.GetBy(request.Param, request.ParamType);
            if (entityInfo == null) 
            return TResult<InstituteInfo>.Fail(ApplicationExMsg.failToFindProgram);

            return TResult<InstituteInfo>.OK(entityInfo);

        }
    }
}
